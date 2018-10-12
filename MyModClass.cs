using MemeLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

/*______________________Info_________________________
 * Template created by theCH33F to work for Two Handed Weapons.
 * 
 * Supports: MemeLoader V0.5.4
 * --------------------------------------------------
 * 
 * ____________________________________________Help!____________________________________________________________________________________________
 * See: Modding: Ask on the Discord! | Below to the `Error Help` region.
 * --------------------------------------------------------------------------------------------------------------------------------------------
 * */

namespace MyCustomWeapon
{

    public class MyCustomWeaponManager
    {

        #region Mod Information
        //Named defined in assembly (Project>ProjectName Properties>Application>Assembly Name)

        public string Creator = "YOU", Version = "V1.0.0"; //V1.0.0 -> Major.Minor.Maintenance[Build]
        public string Description = "Description.";

        //This information is displayed in-game.
        #endregion

        //c = Configuration File Name, m = Mod Name, a = AssetBundle name with extension
        public static string c = "WeaponConfiguration", m = "insertweaponname", a = "customweapon.weapon"; // Change m and a to the corresponding names.
        GameObject spawnInstance;
        #region IGNORE 
        WeaponChance thisWeapon = null;

        public void Init()
        {
            reff = this;

            //This is called when the game starts.

            // ModUtilities.ClearConfig (c,m); //Remove comments this then build to clear the config(Then remove or comment this line), or manually clear it.
            ModUtilities.CreateConfig(c, m);

            SetUpConfig();

            Debug.Log("Loading model...");
            ModUtilities.toInvokeOn.StartCoroutine(ModUtilities.LoadModelFromSource(m, bundleName, modelName, OnModelLoaded));
            Debug.Log(m + " has finished setting up...");
        }

        private void OnModelLoaded(GameObject args)
        {
            weaponPrefab = args;

            thisWeapon = new WeaponChance(weaponPrefab, 100, new bool[]
 {
                canPary,
                canBeParried,
                stickOnDamage,
                hideHandOnGrab,
                isCurrentlyGrabbable,
                setPositionOnGrab,
                setRotationOnGrab,
                canAlsoCut,
                isDamaging
 }, new float[]
 {
                impaleZDamper,
                connectedBMass,
                impaleBreakForce,
                scaleDamage,
                bonusVelocity,
                impaleDepth,
                damageType,
                weaponType,
                addForceToRigidbodyFactor
 });

            ModUtilities.toInvokeOn.AddWeaponToList(thisWeapon);

            Debug.Log(args.name + " has loaded.");
        }

        //When Enemies spawn
        public void OnEnemySetUp(EnemySetupInfo esi)
        {
            if (spawnInstance == null)
            {
                WeaponChance weapon = ModUtilities.GetWeaponFromList(thisWeapon);
                spawnInstance = UnityEngine.Object.Instantiate(weapon.weapon, Player.reff.position + new Vector3(-2, 5, 5), Player.reff.handLeft.transform.rotation);
                spawnInstance.AddComponent<MyWeaponSetUp>().SetUp(weapon, handleLength, standardDrag, grabbedDrag);
                Debug.Log(m + " has spawned.");
            }
        }

        //Setup all the config stuff
        private void SetUpConfig()
        {

            try
            {
                ModUtilities.AddKeyToConfig(c, m, "[REQUIRED]", "WeaponObjectName = Name goes here");
                ModUtilities.AddKeyToConfig(c, m, "[REQUIRED]", "AssetBundleName = " + a);
                ModUtilities.AddKeyToConfig(c, m, "[SPACE]", "");

                #region ints
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "DamageType = 1");
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "WeaponType = 1");
                ModUtilities.AddKeyToConfig(c, m, "[SPACE]", "");
                #endregion

                #region floats
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "HandleLength = 25");
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "ScaleDamage = 1.2");
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "BonusVelocity = 0.7");
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "ImpaleDepth = 1");
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "ImpaleZDamper = 25");
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "ImpaledConnectedBodyMassScale = 10");
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "ImpaledBreakForce = 5000");
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "AddForceToRigidbodyFactor = 0.6");
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "StandardDrag = 1");
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "GrabbedDrag = 0.02");
                #endregion

                #region bools
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "CanPary = true");
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "CanBeParried = true");
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "StickOnDamage = false");
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "HideHandOnGrab = true");
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "IsCurrentlyGrabbable = true");
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "SetPositionOnGrab = true");
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "SetRotationOnGrab = true");
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "CanAlsoCut = true");
                ModUtilities.AddKeyToConfig(c, m, "[OPTION]", "IsDamaging = true");
                #endregion

                modelName = (string)ModUtilities.GetKeyFromConfig(c, m, "WeaponObjectName");
                bundleName = (string)ModUtilities.GetKeyFromConfig(c, m, "AssetBundleName");

                canPary = (bool)ModUtilities.GetKeyFromConfig(c, m, "CanPary");
                canBeParried = (bool)ModUtilities.GetKeyFromConfig(c, m, "CanBeParried");
                stickOnDamage = (bool)ModUtilities.GetKeyFromConfig(c, m, "StickOnDamage");
                hideHandOnGrab = (bool)ModUtilities.GetKeyFromConfig(c, m, "HideHandOnGrab");
                isCurrentlyGrabbable = (bool)ModUtilities.GetKeyFromConfig(c, m, "IsCurrentlyGrabbable");
                setPositionOnGrab = (bool)ModUtilities.GetKeyFromConfig(c, m, "SetPositionOnGrab");
                setRotationOnGrab = (bool)ModUtilities.GetKeyFromConfig(c, m, "SetRotationOnGrab");
                canAlsoCut = (bool)ModUtilities.GetKeyFromConfig(c, m, "CanAlsoCut");
                isDamaging = (bool)ModUtilities.GetKeyFromConfig(c, m, "IsDamaging");

                handleLength = (float)ModUtilities.GetKeyFromConfig(c, m, "HandleLength");
                scaleDamage = (float)ModUtilities.GetKeyFromConfig(c, m, "ScaleDamage");
                bonusVelocity = (float)ModUtilities.GetKeyFromConfig(c, m, "BonusVelocity");
                impaleDepth = (float)ModUtilities.GetKeyFromConfig(c, m, "ImpaleDepth");
                impaleZDamper = (float)ModUtilities.GetKeyFromConfig(c, m, "ImpaleZDamper");
                connectedBMass = (float)ModUtilities.GetKeyFromConfig(c, m, "ImpaledConnectedBodyMassScale");
                impaleBreakForce = (float)ModUtilities.GetKeyFromConfig(c, m, "ImpaledBreakForce");
                addForceToRigidbodyFactor = (float)ModUtilities.GetKeyFromConfig(c, m, "AddForceToRigidbodyFactor");
                standardDrag = (float)ModUtilities.GetKeyFromConfig(c, m, "StandardDrag");
                grabbedDrag = (float)ModUtilities.GetKeyFromConfig(c, m, "GrabbedDrag");

                damageType = (float)ModUtilities.GetKeyFromConfig(c, m, "DamageType");
                weaponType = (float)ModUtilities.GetKeyFromConfig(c, m, "WeaponType");
            }
            catch (Exception e)
            {
                Debug.LogError("UNABLE TO PARSE VALUE, ONE OR MORE MAY HAVE FAILED!\n" + e);
            }
        }

        #region variables
        public static MyCustomWeaponManager reff;
        private GameObject weaponPrefab;
        public string modelName = "", bundleName = "";
        public bool canPary = true, canBeParried = true, stickOnDamage = false, hideHandOnGrab = true, isCurrentlyGrabbable = true, setPositionOnGrab = true, setRotationOnGrab = true, canAlsoCut = true, isDamaging = true;
        public float spawnChance = 25, impaleZDamper = 25, connectedBMass = 10, impaleBreakForce = 5000, scaleDamage = 1.2f, bonusVelocity = 0.7f, impaleDepth = 1, damageType = 1, weaponType = 1, addForceToRigidbodyFactor = 0.6f, handleLength = 25f, standardDrag = 1f, grabbedDrag = 0.02f;
        #endregion
        #endregion
    }


    public class MyWeaponSetUp : MonoBehaviour
    {
        GameObject handle1, handle2, blade;

        public void SetUp(WeaponChance weapon, float handleLength, float standardDrag, float grabbedDrag)
        {
            gameObject.AddComponent<TwoHandedWeaponBase>().type = (WeaponType)weapon.weaponType;

            TwoHandedWeaponBase wb = GetComponent<TwoHandedWeaponBase>();

            blade = transform.GetChild(0).gameObject;
            handle1 = transform.GetChild(1).gameObject;
            handle2 = transform.GetChild(2).gameObject;

            if (handle1 == null || blade == null || handle2 == null)
            {
                Debug.LogError("BLADE OR HANDLE IS MISSING!");
                return;
            }

            //Weapon Handle 1, the back grip
            handle1.AddComponent<TwoHandedWeaponBackGrip>();
            handle1.AddComponent<WeaponHandle>();

            WeaponHandle wh1 = handle1.GetComponent<WeaponHandle>();
            TwoHandedWeaponBackGrip twoHandBg = handle1.GetComponent<TwoHandedWeaponBackGrip>();

            wb.grabbable = wh1;
            wb.canParry = weapon.canPary;
            wb.canBeParried = weapon.canBeParried;
            wb.addForceToRigidbodyFactor = weapon.addForceToRigidbodyFactor;

            wh1.grabRigidbody = wh1.GetComponent<Rigidbody>();
            wh1.hideHandModelOnGrab = weapon.hideHandOnGrab;
            wh1.isCurrentlyGrabbale = weapon.isCurrentlyGrabbable;

            wh1.setPositionOnGrab = true;
            wh1.setRotationOnGrab = false;

            wh1.doNotLockRotation = true;
            wh1.doNotLockPosition = false;

            wh1.isBackHandGrip = true;
            wh1.isFrontHandGrip = false;

            wh1.twoHandGripStrengthPivot = 7.5f;
            wh1.twoHandGripMaxStrength = 50000f;

            wh1.weaponBase = wb;

            //Weapon Handle 2, where you'll orient, the front grip
            handle2.AddComponent<WeaponHandle>();
            handle2.AddComponent<TwoHandedWeaponFrontGrip>();

            WeaponHandle wh2 = handle2.GetComponent<WeaponHandle>();
            TwoHandedWeaponFrontGrip twoHandFg = handle2.GetComponent<TwoHandedWeaponFrontGrip>();


            twoHandFg.weaponJoint = handle2.GetComponent<ConfigurableJoint>();
            twoHandFg.handleLength = handleLength;
            twoHandFg.backGrabbable = wh1;
            twoHandFg.backGrip = wh1.transform;
            twoHandFg.handleJoint = handle1.GetComponent<ConfigurableJoint>();
            twoHandFg.handle = handle1.transform;
            twoHandFg.standardDrag = standardDrag;
            twoHandFg.grabbedDrag = grabbedDrag;
            twoHandFg.keepWeaponDistance = false;
            twoHandFg.scaleDamageAndVelocityByGripStrength = true;
            twoHandFg.rangeLimit = new Vector2(4, 15);
            twoHandFg.damager = blade.GetComponent<DamagerRigidbody>();
            twoHandFg.headAlignRotationOffset = 90f;

            wb.frontGrip = wh2;

            wh2.grabRigidbody = wh2.GetComponent<Rigidbody>();
            wh2.hideHandModelOnGrab = weapon.hideHandOnGrab;
            wh2.isCurrentlyGrabbale = weapon.isCurrentlyGrabbable;
            wh2.setPositionOnGrab = true;
            wh2.setRotationOnGrab = weapon.setRotationOnGrab;
            wh2.isFrontHandGrip = true;
            wh2.isBackHandGrip = false;
            wh2.doNotLockRotation = true;
            wh2.addForceFactor = 0.5f;

            DamagerRigidbody drb = blade.AddComponent<DamagerRigidbody>();

            drb.scaleDamage = weapon.scaleDamage;
            drb.canAlsoCut = weapon.canAlsoCut;
            drb.bonusVelocity = weapon.bonusVelocity;
            drb.isDamaging = weapon.isDamaging;
            drb.impaleDepth = weapon.impaleDepth;
            drb.damageType = (DamageType)weapon.damageType;

            handle1.AddComponent<FootStepSound>().soundEffectName = "WeaponDrop";
            handle1.GetComponent<FootStepSound>().minVolumeToTrigger = 0.05f;

            return;
        }
    }
}

#region Error help

/*Q = Question
 *A = Answer
 *O = Optional
 *I = Additional Information
 * - - - - - - - - - - - - - 
 * -Q: It says I'm missing an assembly reference?-
 * ==============================================
 * A: View>Solution Explorer>References>Right-click>Add>Clear all(if any show up)(Right-click one and clear all)>Browse>Project root>Plugins>Select All.
 * 
 * -Q: My mod won't load!- 
 * ========================
 *  A: Did you remove Init()? If not, everything should work, it'll be your code, double check!
 *  I: I keep dlSpy(.dll deassembler) open so I can see the source to understand what I'm modifying.
 *  
 *  -Q: I accidentally broke the game, help!-
 *  =========================================
 *  A: Delete: GORN_Data>Managed>Assembly-CSharp.dll and the most recent mod you broke it with.
 *  O: Verify file integrity, launching the game will start this automatically.
 */

#endregion