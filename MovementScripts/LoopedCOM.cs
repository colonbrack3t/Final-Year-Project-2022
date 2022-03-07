using UnityEngine;
public class LoopedCOM : LoopedObjects
{
    // Headset transform
    [SerializeField] private Transform head;
    //overall sensitivity of sway
    [SerializeField] private float sensitivity = 1;
    //individual sensitivity of head and board
    [SerializeField] private float head_modifier = 20f, board_modifier = 0.5f, LPF_Threshold = 0.5f;
    // location of user's ankle
    [SerializeField] private Transform ankleJoint;
    //Balance board sensor object, has realtime sensor values
    [SerializeField] private BalanceBoardSensor bbs;
    public bool enableSway = false;
    Vector2 last_board_centre = new Vector2(0, 0);
    Vector3 last_head_movement = new Vector3(0, 0, 0);
    protected Vector3 BoardReading()
    {
        double f1 = bbs.rwTopLeft, f2 = bbs.rwTopRight, b1 = bbs.rwBottomLeft, b2 = bbs.rwBottomRight;
        if (f1 + f2 + b1 + b2 < 20) return new Vector3(0, 0, 0);

        double topright_backright = f1 / (f1 + b1);
        double topleft_backleft = f2 / (f2 + b2);
        double topright_topleft = f1 / (f1 + f2);
        double backright_backleft = b1 / (b1 + b2);

        double centre_x = (topright_topleft + backright_backleft) / 2;
        double centre_y = (topright_backright + topleft_backleft) / 2;

        Vector2 board_centre = new Vector2((float)centre_x, (float)centre_y);

        Vector2 change_centre = board_centre - last_board_centre;

        last_board_centre = board_centre;

        Vector2 filtered_change = change_centre;//LPF_Vector2(change_centre);

        filtered_change *= board_modifier;

        return new Vector3(filtered_change.x, 0, filtered_change.y);
    }
    // LPF based on total vector mag
    Vector2 LPF_Vector2(Vector2 c)
    {
        if (c.magnitude > LPF_Threshold * LPF_Threshold)
        {   
            Vector3 unit = c / c.magnitude;
            Vector3 excess = (unit * LPF_Threshold);
            return (unit * LPF_Threshold) + (excess * 0.4f );
            //return (c / c.magnitude) * LPF_Threshold;
        }
        return c;
    }
    protected Vector3 HeadReading()
    {
        Vector3 change_in_head = head.position - last_head_movement;
        last_head_movement = head.position;
        Vector3 modchange = change_in_head * head_modifier;
        return modchange;

    }

}