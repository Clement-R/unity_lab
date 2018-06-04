using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBehaviour : MonoBehaviour {

    public Gradient baseColor;
    public Gradient actionColor;

    public GameObject lightning;

    private bool _canFire = true;
    private LineRenderer _line;


    void Start () {
        _line = GetComponentInChildren<LineRenderer>();

        _line.SetPosition(0, transform.position);
        _line.SetPosition(1, transform.position);
        
        //_line.textureMode = LineTextureMode.Tile;
        //_line.material.SetTextureScale("_MainTex", new Vector2(10f, 1.0f));
    }
	
    private void OnMouseDown()
    {
        if(_canFire)
        {
            // TOOD : draw line
        }
    }

    private void OnMouseUp()
    {
        if(_canFire)
        {
            _canFire = false;

            StartCoroutine(Fire());

            // TODO : Fire
        }
    }

    private IEnumerator Fire()
    {
        GameObject light = Instantiate(lightning, transform);
        LightningBehaviour lightningBehaviour = light.GetComponent<LightningBehaviour>();
        lightningBehaviour.FireTo(new Vector2(_line.GetPosition(1).x, _line.GetPosition(1).y));

        _line.colorGradient = actionColor;
        
        yield return new WaitForSeconds(0.5f);

        _line.SetPosition(1, transform.position);

        _line.colorGradient = baseColor;

        // TODO : Debug
        _canFire = true;
    }

    private void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(new Vector2(mousePos.x, mousePos.y), transform.position);
        
        //_line.materials[0].mainTextureScale = new Vector3(distance, 1, 1);

        _line.SetPosition(1, new Vector2(mousePos.x, mousePos.y));

        _line.material.mainTextureScale = new Vector2((int)Vector2.Distance(_line.GetPosition(0), _line.GetPosition(1)), 1);
    }
}
