using UnityEngine;
using System.Collections;

public class AE_AnimatorEvents : MonoBehaviour
{
    public AE_EffectAnimatorProperty Effect1;
    public AE_EffectAnimatorProperty Effect2;
    public AE_EffectAnimatorProperty Effect3;
    public AE_EffectAnimatorProperty Effect4;

    // Bow Effects
    public GameObject Poison_Shot;
    public GameObject Poison_Bow;
    public GameObject Fire_Shot;
    public GameObject Fire_Arrow;
    public GameObject Ice_Shot;
    public GameObject Ice_Arrow;
    public GameObject Dark_Shot;
    public GameObject Dark_Bow;

    // Magic Effects
    public GameObject Dark_Magic;
    public GameObject Fire_Magic;
    public GameObject Ice_Magic;

    // Position & Rotation
    public GameObject Bow_Center;
    public GameObject Bow_Rotation;
    public GameObject Magic_Center;
    public GameObject Magic_Rotation;


    public GameObject Bow;
    public GameObject Arrow;

    public WeaponAttackType attack_type;
    public WeaponType weapon_type;

    [HideInInspector] public float HUE = -1;

    [System.Serializable]
    public class AE_EffectAnimatorProperty
    {
        [HideInInspector] public RuntimeAnimatorController TargetAnimation;
        public GameObject Prefab;
        public Transform BonePosition;
        public Transform BoneRotation;
        public float DestroyTime = 10;
        [HideInInspector] public GameObject CurrentInstance;
    }

    void InstantiateEffect(AE_EffectAnimatorProperty effect, bool returnIfCreatedInstance = false)
    {
        if (effect.Prefab == null) return;
       // if (returnIfCreatedInstance && effect.CurrentInstance!= null && GameObject.Find(effect.CurrentInstance.name)) return;

        if (effect.BonePosition != null && effect.BoneRotation != null)
            effect.CurrentInstance = Instantiate(effect.Prefab, effect.BonePosition.position, effect.BoneRotation.rotation);
        else effect.CurrentInstance = Instantiate(effect.Prefab);

        if(effect.TargetAnimation != null)
        {
            effect.CurrentInstance.GetComponent<Animator>().runtimeAnimatorController = effect.TargetAnimation;
        }

        if (Bow != null)
        {
            var setMeshToEffect = effect.CurrentInstance.GetComponent<AE_SetMeshToEffect>();
            if (setMeshToEffect != null && setMeshToEffect.MeshType == AE_SetMeshToEffect.EffectMeshType.Bow)
            {
                setMeshToEffect.Mesh = Bow;
            }
        }

        if (Arrow != null)
        {
            var setMeshToEffect = effect.CurrentInstance.GetComponent<AE_SetMeshToEffect>();
            if (setMeshToEffect != null && setMeshToEffect.MeshType == AE_SetMeshToEffect.EffectMeshType.Arrow)
            {
                setMeshToEffect.Mesh = Arrow;
            }
        }


        if(HUE > -0.9f)
        {
            UpdateColor(effect);
        }

        if (effect.DestroyTime > 0.001f) Destroy(effect.CurrentInstance, effect.DestroyTime);
    }

    public void ActivateEffect1()
    {
        InstantiateEffect(Effect1);
    }

    public void ActivateEffect2()
    {
        InstantiateEffect(Effect2);
    }

    public void ActivateEffect3()
    {
        InstantiateEffect(Effect3, true);
    }

    public void ActivateEffect4()
    {
        InstantiateEffect(Effect4);
    }

    private void UpdateColor(AE_EffectAnimatorProperty effect)
    {
        var settingColor = effect.CurrentInstance.GetComponent<AE_EffectSettingColor>();
        if (settingColor == null) settingColor = effect.CurrentInstance.AddComponent<AE_EffectSettingColor>();
        var hsv = AE_ColorHelper.ColorToHSV(settingColor.Color);
        hsv.H = HUE;
        settingColor.Color = AE_ColorHelper.HSVToColor(hsv);
    }

    private void Update()
    {
        attack_type = GetComponent<PlayerCurrentWeapon>().attack_type;
        weapon_type = GetComponent<PlayerCurrentWeapon>().weaponType;
        if(weapon_type == WeaponType.Bow)
        {
            Effect1.BonePosition = Bow_Center.transform;
            Effect1.BoneRotation = Bow_Rotation.transform;
            Effect2.BonePosition = Bow_Center.transform;
            Effect2.BoneRotation = Bow_Rotation.transform;
        }
        if(weapon_type == WeaponType.Staff)
        {
            Effect1.BonePosition = Magic_Center.transform;
            Effect1.BoneRotation = Magic_Center.transform;
        }


        if(attack_type == WeaponAttackType.Dark)
        {
            if(weapon_type == WeaponType.Bow)
            {
                Effect1.Prefab = Dark_Shot;
                Effect2.Prefab = Dark_Bow;
            }
            if(weapon_type == WeaponType.Staff)
            {
                Effect1.Prefab = Dark_Magic;
            }
        }
        if(attack_type == WeaponAttackType.Poison)
        {
            if (weapon_type == WeaponType.Bow)
            {
                Effect1.Prefab = Poison_Shot;
                Effect2.Prefab = Poison_Bow;
            }

        }
        if(attack_type == WeaponAttackType.Fire)
        {
            if(weapon_type == WeaponType.Bow)
            {
                Effect1.Prefab = Fire_Shot;
                Effect2.Prefab = Fire_Arrow;
            }
            if(weapon_type == WeaponType.Staff)
            {
                Effect1.Prefab = Fire_Magic;
            }
        }
        if(attack_type == WeaponAttackType.Ice)
        {
            if (weapon_type == WeaponType.Bow)
            {
                Effect1.Prefab = Ice_Shot;
                Effect2.Prefab = Ice_Arrow;
            }
            if(weapon_type == WeaponType.Staff)
            {
                Effect1.Prefab = Ice_Magic;
            }
        }
    }

}
