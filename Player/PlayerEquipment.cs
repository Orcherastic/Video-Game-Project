using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public Animator MyAnimator { get; set; }
    private Animator parentAnimator;
    private SpriteRenderer spriteRenderer;
    private AnimatorOverrideController animatorOverrideController;

    public ArmorType equipType;

    public SpriteRenderer SpriteRenderer
    {
        get { return spriteRenderer; }
        set { spriteRenderer = value; }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentAnimator = GetComponentInParent<Animator>();
        MyAnimator = GetComponent<Animator>();

        animatorOverrideController = new AnimatorOverrideController(MyAnimator.runtimeAnimatorController);

        MyAnimator.runtimeAnimatorController = animatorOverrideController;
    }

    public void SetXAndY(float x, float y)
    {
        MyAnimator.SetFloat("Horizontal", x);
        MyAnimator.SetFloat("Vertical", y);
    }
    public void SetRunWalkBools(string running, bool b1, string walking, bool b2)
    {
        MyAnimator.SetBool(running, b1);
        MyAnimator.SetBool(walking, b2);
    }

    public void SetAnimatorBool(string name, bool b1)
    {
        MyAnimator.SetBool(name, b1);
    }

    public void SetAnimatorTrigger(string trigger)
    {
        MyAnimator.SetTrigger(trigger);
    }

    public void Equip(AnimationClip[] animations)
    {
        spriteRenderer.color = Color.white;
        // ACCESSORY EQUIPMENTS ONLY HAVE 1 ANIMATION
        if(equipType == ArmorType.Accessory)
        {
            #region ACCESSORY ANIMATIONS
            // IDLE
            animatorOverrideController["player_idle_down"] = animations[0];
            animatorOverrideController["player_idle_left"] = animations[0];
            animatorOverrideController["player_idle_right"] = animations[0];
            animatorOverrideController["player_idle_up"] = animations[0];
            // RUN
            animatorOverrideController["player_run_down"] = animations[0];
            animatorOverrideController["player_run_left"] = animations[0];
            animatorOverrideController["player_run_right"] = animations[0];
            animatorOverrideController["player_run_up"] = animations[0];
            // WALK
            animatorOverrideController["player_walk_down"] = animations[0];
            animatorOverrideController["player_walk_left"] = animations[0];
            animatorOverrideController["player_walk_right"] = animations[0];
            animatorOverrideController["player_walk_up"] = animations[0];
            // STAGGER
            animatorOverrideController["player_stagger_down"] = animations[0];
            animatorOverrideController["player_stagger_left"] = animations[0];
            animatorOverrideController["player_stagger_right"] = animations[0];
            animatorOverrideController["player_stagger_up"] = animations[0];
            // ATTACK1
            animatorOverrideController["player_attack_down"] = animations[0];
            animatorOverrideController["player_attack_left"] = animations[0];
            animatorOverrideController["player_attack_right"] = animations[0];
            animatorOverrideController["player_attack_up"] = animations[0];
            // ATTACK2
            animatorOverrideController["player_attack_down_2"] = animations[0];
            animatorOverrideController["player_attack_left_2"] = animations[0];
            animatorOverrideController["player_attack_right_2"] = animations[0];
            animatorOverrideController["player_attack_up_2"] = animations[0];
            // ATTACK3
            animatorOverrideController["player_attack_down_3"] = animations[0];
            animatorOverrideController["player_attack_left_3"] = animations[0];
            animatorOverrideController["player_attack_right_3"] = animations[0];
            animatorOverrideController["player_attack_up_3"] = animations[0];
            // USE ITEM
            animatorOverrideController["player_use_item_drinking"] = animations[0];
            animatorOverrideController["player_use_item_eating"] = animations[0];
            animatorOverrideController["player_use_item_repair_weapon"] = animations[0];
            // RECIEVE ITEM
            animatorOverrideController["player_recieve_item"] = animations[0];

            // BATTLE IDLE
            animatorOverrideController["player_battle_idle_down"] = animations[0];
            animatorOverrideController["player_battle_idle_left"] = animations[0];
            animatorOverrideController["player_battle_idle_right"] = animations[0];
            animatorOverrideController["player_battle_idle_up"] = animations[0];
            // BATTLE RUN                                           
            animatorOverrideController["player_battle_run_down"] = animations[0];
            animatorOverrideController["player_battle_run_left"] = animations[0];
            animatorOverrideController["player_battle_run_right"] = animations[0];
            animatorOverrideController["player_battle_run_up"] = animations[0];
            // BATTLE WALK                                          
            animatorOverrideController["player_battle_walk_down"] = animations[0];
            animatorOverrideController["player_battle_walk_left"] = animations[0];
            animatorOverrideController["player_battle_walk_right"] = animations[0];
            animatorOverrideController["player_battle_walk_up"] = animations[0];
            // BATTLE STAGGER
            animatorOverrideController["player_battle_stagger_down"] = animations[0];
            animatorOverrideController["player_battle_stagger_left"] = animations[0];
            animatorOverrideController["player_battle_stagger_right"] = animations[0];
            animatorOverrideController["player_battle_stagger_up"] = animations[0];
            #endregion
        }
        else
        {
            // WEAPON HAS NO NON BATTLE ANIMATIONS
            if (equipType != ArmorType.Weapon)
            {
                #region NON WEAPON ANIMATIONS
                // IDLE
                animatorOverrideController["player_idle_down"] = animations[0];
                animatorOverrideController["player_idle_left"] = animations[1];
                animatorOverrideController["player_idle_right"] = animations[2];
                animatorOverrideController["player_idle_up"] = animations[3];
                // RUN
                animatorOverrideController["player_run_down"] = animations[4];
                animatorOverrideController["player_run_left"] = animations[5];
                animatorOverrideController["player_run_right"] = animations[6];
                animatorOverrideController["player_run_up"] = animations[7];
                // WALK
                animatorOverrideController["player_walk_down"] = animations[8];
                animatorOverrideController["player_walk_left"] = animations[9];
                animatorOverrideController["player_walk_right"] = animations[10];
                animatorOverrideController["player_walk_up"] = animations[11];
                // STAGGER
                animatorOverrideController["player_stagger_down"] = animations[12];
                animatorOverrideController["player_stagger_left"] = animations[13];
                animatorOverrideController["player_stagger_right"] = animations[14];
                animatorOverrideController["player_stagger_up"] = animations[15];
                // USE ITEM
                animatorOverrideController["player_use_item_drinking"] = animations[0];
                animatorOverrideController["player_use_item_eating"] = animations[0];
                animatorOverrideController["player_use_item_repair_weapon"] = animations[0];
                // RECIEVE ITEM
                animatorOverrideController["player_recieve_item"] = animations[0];

                // BATTLE IDLE
                animatorOverrideController["player_battle_idle_down"] = animations[28];
                animatorOverrideController["player_battle_idle_left"] = animations[29];
                animatorOverrideController["player_battle_idle_right"] = animations[30];
                animatorOverrideController["player_battle_idle_up"] = animations[31];
                // BATTLE STAGGER
                animatorOverrideController["player_battle_stagger_down"] = animations[12];
                animatorOverrideController["player_battle_stagger_left"] = animations[13];
                animatorOverrideController["player_battle_stagger_right"] = animations[14];
                animatorOverrideController["player_battle_stagger_up"] = animations[15];

                // CHESTPLATE HAS EXTRA ANIMATIONS FOR WALKING AND RUNNING
                if (equipType == ArmorType.Chestplate)
                {
                    // BATTLE RUN
                    animatorOverrideController["player_battle_run_down"] = animations[32];
                    animatorOverrideController["player_battle_run_left"] = animations[33];
                    animatorOverrideController["player_battle_run_right"] = animations[34];
                    animatorOverrideController["player_battle_run_up"] = animations[35];
                    // BATTLE WALK
                    animatorOverrideController["player_battle_walk_down"] = animations[36];
                    animatorOverrideController["player_battle_walk_left"] = animations[37];
                    animatorOverrideController["player_battle_walk_right"] = animations[38];
                    animatorOverrideController["player_battle_walk_up"] = animations[39];
                }
                // OTHER EQUIPMENTS HAVE THE SAME WALK AND RUN ANIMATIONS
                else
                {
                    // BATTLE RUN
                    animatorOverrideController["player_battle_run_down"] = animations[4];
                    animatorOverrideController["player_battle_run_left"] = animations[5];
                    animatorOverrideController["player_battle_run_right"] = animations[6];
                    animatorOverrideController["player_battle_run_up"] = animations[7];
                    // BATTLE WALK
                    animatorOverrideController["player_battle_walk_down"] = animations[8];
                    animatorOverrideController["player_battle_walk_left"] = animations[9];
                    animatorOverrideController["player_battle_walk_right"] = animations[10];
                    animatorOverrideController["player_battle_walk_up"] = animations[11];
                }
                #endregion
            }
            // WEAPON ANIMATIONS ARE ONLY BATTLE ANIMATIONS
            else
            {
                #region WEAPON ANIMATIONS

                // BATTLE IDLE
                animatorOverrideController["player_battle_idle_down"] = animations[0];
                animatorOverrideController["player_battle_idle_left"] = animations[1];
                animatorOverrideController["player_battle_idle_right"] = animations[2];
                animatorOverrideController["player_battle_idle_up"] = animations[3];
                // BATTLE RUN
                animatorOverrideController["player_battle_run_down"] = animations[4];
                animatorOverrideController["player_battle_run_left"] = animations[5];
                animatorOverrideController["player_battle_run_right"] = animations[6];
                animatorOverrideController["player_battle_run_up"] = animations[7];
                // BATTLE WALK
                animatorOverrideController["player_battle_walk_down"] = animations[8];
                animatorOverrideController["player_battle_walk_left"] = animations[9];
                animatorOverrideController["player_battle_walk_right"] = animations[10];
                animatorOverrideController["player_battle_walk_up"] = animations[11];
                // BATTLE STAGGER
                animatorOverrideController["player_battle_stagger_down"] = animations[12];
                animatorOverrideController["player_battle_stagger_left"] = animations[13];
                animatorOverrideController["player_battle_stagger_right"] = animations[14];
                animatorOverrideController["player_battle_stagger_up"] = animations[15];
                // REPAIR WEAPON
                animatorOverrideController["player_use_item_repair_weapon"] = animations[28];
                // HEAVY STRIKE
                animatorOverrideController["player_heavy_strike_down"] = animations[29];
                animatorOverrideController["player_heavy_strike_left"] = animations[30];
                animatorOverrideController["player_heavy_strike_right"] = animations[31];
                animatorOverrideController["player_heavy_strike_up"] = animations[32];

                // USE ITEM
                //animatorOverrideController["player_use_item_drinking"] = null;
                //animatorOverrideController["player_use_item_eating"] = null;
                // RECIEVE ITEM
                //animatorOverrideController["player_recieve_item"] = null;
                #endregion
            }

            // ATTACK ANIMATIONS ARE THE SAME
            #region ATTACK ANIMATIONS
            // ATTACK1
            animatorOverrideController["player_attack_down"] = animations[16];
            animatorOverrideController["player_attack_left"] = animations[19];
            animatorOverrideController["player_attack_right"] = animations[22];
            animatorOverrideController["player_attack_up"] = animations[25];
            // ATTACK2
            animatorOverrideController["player_attack_down_2"] = animations[17];
            animatorOverrideController["player_attack_left_2"] = animations[20];
            animatorOverrideController["player_attack_right_2"] = animations[23];
            animatorOverrideController["player_attack_up_2"] = animations[26];
            // ATTACK3
            animatorOverrideController["player_attack_down_3"] = animations[18];
            animatorOverrideController["player_attack_left_3"] = animations[21];
            animatorOverrideController["player_attack_right_3"] = animations[24];
            animatorOverrideController["player_attack_up_3"] = animations[27];
            #endregion
        }
    }

    public void Dequip()
    {
        #region ALL ANIMATIONS
        // IDLE
        animatorOverrideController["player_idle_down"] = null;
        animatorOverrideController["player_idle_left"] = null;
        animatorOverrideController["player_idle_right"] = null;
        animatorOverrideController["player_idle_up"] = null;
        // RUN
        animatorOverrideController["player_run_down"] = null;
        animatorOverrideController["player_run_left"] = null;
        animatorOverrideController["player_run_right"] = null;
        animatorOverrideController["player_run_up"] = null;
        // WALK
        animatorOverrideController["player_walk_down"] = null;
        animatorOverrideController["player_walk_left"] = null;
        animatorOverrideController["player_walk_right"] = null;
        animatorOverrideController["player_walk_up"] = null;
        // STAGGER
        animatorOverrideController["player_stagger_down"] = null;
        animatorOverrideController["player_stagger_left"] = null;
        animatorOverrideController["player_stagger_right"] = null;
        animatorOverrideController["player_stagger_up"] = null;
        // ATTACK1
        animatorOverrideController["player_attack_down"] = null;
        animatorOverrideController["player_attack_left"] = null;
        animatorOverrideController["player_attack_right"] = null;
        animatorOverrideController["player_attack_up"] = null;
        // ATTACK2
        animatorOverrideController["player_attack_down_2"] = null;
        animatorOverrideController["player_attack_left_2"] = null;
        animatorOverrideController["player_attack_right_2"] = null;
        animatorOverrideController["player_attack_up_2"] = null;
        // ATTACK3
        animatorOverrideController["player_attack_down_3"] = null;
        animatorOverrideController["player_attack_left_3"] = null;
        animatorOverrideController["player_attack_right_3"] = null;
        animatorOverrideController["player_attack_up_3"] = null;
        // USE ITEM
        animatorOverrideController["player_use_item_drinking"] = null;
        animatorOverrideController["player_use_item_eating"] = null;
        animatorOverrideController["player_use_item_repair_weapon"] = null;
        // RECIEVE ITEM
        animatorOverrideController["player_recieve_item"] = null;

        // BATTLE IDLE
        animatorOverrideController["player_battle_idle_down"] = null;
        animatorOverrideController["player_battle_idle_left"] = null;
        animatorOverrideController["player_battle_idle_right"] =null;
        animatorOverrideController["player_battle_idle_up"] = null;
        // BATTLE RUN                                           
        animatorOverrideController["player_battle_run_down"] = null;
        animatorOverrideController["player_battle_run_left"] = null;
        animatorOverrideController["player_battle_run_right"] = null;
        animatorOverrideController["player_battle_run_up"] = null;
        // BATTLE WALK                                          
        animatorOverrideController["player_battle_walk_down"] = null;
        animatorOverrideController["player_battle_walk_left"] = null;
        animatorOverrideController["player_battle_walk_right"] =null;
        animatorOverrideController["player_battle_walk_up"] = null;
        // BATTLE STAGGER
        animatorOverrideController["player_battle_stagger_down"] = null;
        animatorOverrideController["player_battle_stagger_left"] = null;
        animatorOverrideController["player_battle_stagger_right"] = null;
        animatorOverrideController["player_battle_stagger_up"] = null;
        #endregion

        Color c = spriteRenderer.color;
        c.a = 0;
        spriteRenderer.color = c;
    }
}
