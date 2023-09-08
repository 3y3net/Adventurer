using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropInstance : MonoBehaviour {

    //MUST match the DropDatabase order object
    public enum DropItem { Knife, TwoPistols, Shotgun, Repetition, Shield, MagazineTwoPistol, MagazineFragmntation, MagazineRepetition, MagazineShield, Grenade,
                            Health50, Health100, Armor25, Armor50, Armor100, OrangeKey, BlueKey, GreenKey}

    public DropItem dropItem;
    
    //public Color highlightColor = Color.cyan;    
    public float prefabScale = 1.5f;
    public int mainValue, auxValue;    

    public float rotateSpeed = 100f;

    float collisionRadio = 1f;

    SphereCollider col;
    //OutlinedObject outline;
    DropObject dropObject;
    Transform dropObjectTransform, estrellitas;

	void Start () {
        dropObject = DropDatabase.instance.AllDrops[(int)dropItem];
        col = GetComponent<SphereCollider>();
        if (col==null)        
            col = gameObject.AddComponent<SphereCollider>();
        col.radius = collisionRadio;
        col.isTrigger = true;

        dropObjectTransform = GameObject.Instantiate(dropObject.prefab, transform);
        dropObjectTransform.localScale = new Vector3(prefabScale, prefabScale, prefabScale);

        estrellitas = GameObject.Instantiate(GameLogic.instance.estrellitas, transform);        
        /*
       outline = dropObjectTransform.gameObject.GetComponent<OutlinedObject>();
       if (outline == null)
           outline = dropObjectTransform.gameObject.AddComponent<OutlinedObject>();
       outline.outlineMode = OutlinedObject.OutlineMode.AllwaysOn;
       outline.outlineColor = highlightColor;
       */
    }
    

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "Player")
        {

            if(dropObject.pickSound!=null)
            {
                GameLogic.instance.FX.clip = dropObject.pickSound;
                GameLogic.instance.FX.Play();
            }

            if(dropObject.invetoryIcon!=null)
            {
                GameLogic.instance.SetInventorySlot(dropObject.invetoryIcon, dropItem);
            }

            /*
            if(dropObject.dropType==DropType.weapon)
                DropDatabase.instance.selector.PickWeapon(dropObject.internalIndex, mainValue, auxValue);
            else if (dropObject.dropType == DropType.magazine)
                DropDatabase.instance.selector.PickAmmo(dropObject.internalIndex, mainValue);
            else if (dropObject.dropType == DropType.grenade)
                DropDatabase.instance.selector.PickGrenade(mainValue);
            else if (dropObject.dropType == DropType.health)
                DropDatabase.instance.controller.PickHealth(mainValue);
            else if (dropObject.dropType == DropType.Armor)
                DropDatabase.instance.controller.PickArmor(mainValue);
            else if (dropObject.dropType == DropType.Key)
                DropDatabase.instance.controller.PickKey(mainValue);
            */
            Object.Destroy(estrellitas, 0.95f);
            Object.Destroy(gameObject, 0.1f);
        }        
    }

    // Update is called once per frame
    void Update () {
        transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed, Space.World);
    }
}
