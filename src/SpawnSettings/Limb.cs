using UnityEngine;

public class Limb
{
    public string Name;
    public Color Color;
    public Rect sizeRect;
    public bool Draw;

    public float health;

    public float Health
    {
        get => health;
        set
        {
            // Clamp once here
            health = Mathf.Clamp(value, 0f, 100f);
            UpdateColor(health);
            //Effects(health);
        }
    }

    public Limb(string name, Color color, Rect rect, float health = 100f, bool draw = true)
    {
        this.Name = name;
        this.sizeRect = rect;
        this.Draw = draw;
        this.Health = health; // Use the setter to clamp + set color
    }

    private void UpdateColor(float h)
    {
        float t = h / 100f;

        if (h <= 0f)
        {
            Color = Color.black;
        }
        else if (t < 0.5f)
        {
            // 0-50% health: black to red
            Color = Color.Lerp(Color.black, Color.red, t * 2f);
        }
        else
        {
            // 50-100% health: red to white
            Color = Color.Lerp(Color.red, Color.white, (t - 0.5f) * 2f);
        }
    }

    //private void Effects(float hp)
    //{
    //    switch (this.Name)
    //    {
    //        case "Head":
    //            HeadEffect();
    //            break;
    //        case "Foot_L":
    //            FootEffect();
    //            break;
    //        case "Foot_R":
    //            FootEffect();
    //            break;
    //        case "Arm_L":
    //            ArmEffect();
    //            break;
    //        case "Arm_R":
    //            ArmEffect();
    //            break;
    //        case "Torso":
    //            TorsoEffect();
    //            break;

    //    }
    //}

    public void Damage(float dmg)
    {
        //Debug.Log($"Before: {Name} health = {Health}, taking {dmg} damage");
        Health -= dmg;
        //Debug.Log($"After: {Name} health = {Health}, color = {Color}");
    }
}
