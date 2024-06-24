using System;
using System.Runtime.InteropServices;

namespace PsMoveAPI
{
    public class ControllerHelper
    {
        #region enums
        public enum PSMove_Bool
        {
            PSMove_False = 0, /*!< False, Failure, Disabled (depending on context) */
            PSMove_True = 1, /*!< True, Success, Enabled (depending on context) */
        };

        public enum PSMove_Version
        {
            /**
             * Version format: AA.BB.CC = 0xAABBCC
             *
             * Examples:
             *  3.0.1 = 0x030001
             *  4.2.11 = 0x04020B
             **/
            PSMOVE_CURRENT_VERSION = 0x04000C, /*!< Current version, see psmove_init() 4.0.12 */
        }

        // Not entirely sure why some of these buttons (R3/L3) are exposed...
        public enum PSMoveButton
        {
            L2 = 1 << 0x00,
            R2 = 1 << 0x01,
            L1 = 1 << 0x02,
            R1 = 1 << 0x03,
            Triangle = 1 << 0x04,
            Circle = 1 << 0x05,
            Cross = 1 << 0x06,
            Square = 1 << 0x07,
            Select = 1 << 0x08,
            L3 = 1 << 0x09,
            R3 = 1 << 0x0A,
            Start = 1 << 0x0B,
            Up = 1 << 0x0C,
            Right = 1 << 0x0D,
            Down = 1 << 0x0E,
            Left = 1 << 0x0F,
            PS = 1 << 0x10,
            Move = 1 << 0x13,
            Trigger = 1 << 0x14,    /* We can use this value with IsButtonDown() (or the events) to get
                             * a binary yes/no answer about if the trigger button is down at all.
                             * For the full integer/analog value of the trigger, see the corresponding property below.
                             */
        };

        // Used by psmove_get_battery().
        public enum PSMove_Battery_Level
        {
            Batt_MIN = 0x00, /*!< Battery is almost empty (< 20%) */
            Batt_20Percent = 0x01, /*!< Battery has at least 20% remaining */
            Batt_40Percent = 0x02, /*!< Battery has at least 40% remaining */
            Batt_60Percent = 0x03, /*!< Battery has at least 60% remaining */
            Batt_80Percent = 0x04, /*!< Battery has at least 80% remaining */
            Batt_MAX = 0x05, /*!< Battery is fully charged (not on charger) */
            Batt_CHARGING = 0xEE, /*!< Battery is currently being charged */
            Batt_CHARGING_DONE = 0xEF, /*!< Battery is fully charged (on charger) */
        };
        #endregion

        [DllImport("psmoveapi.dll")]
        public static extern PSMove_Bool psmove_init(PSMove_Version version);

        [DllImport("psmoveapi.dll")]
        public static extern uint psmove_poll(IntPtr move);

        //connection
        [DllImport("psmoveapi.dll")]
        public static extern int psmove_pair(IntPtr move);

        [DllImport("psmoveapi.dll")]
        public static extern void psmove_disconnect(IntPtr move);

        [DllImport("psmoveapi.dll")]
        public static extern int psmove_count_connected();

        [DllImport("psmoveapi.dll")]
        public static extern IntPtr psmove_connect_by_id(int id);

        //buttons
        [DllImport("psmoveapi.dll")]
        public static extern uint psmove_get_buttons(IntPtr move);

        //leds
        [DllImport("psmoveapi.dll")]
        public static extern void psmove_set_leds(IntPtr move, byte r, byte g, byte b);

        [DllImport("psmoveapi.dll")]
        public static extern int psmove_update_leds(IntPtr move);

        [DllImport("psmoveapi.dll")]
        public static extern PSMove_Battery_Level psmove_get_battery(IntPtr move);

        //movement data 
        [DllImport("psmoveapi.dll")]
        public static extern void psmove_get_gyroscope(IntPtr move, ref int gx, ref int gy, ref int gz);
        
        [DllImport("psmoveapi.dll")]
        public static extern void psmove_get_gyroscope_frame(IntPtr move, ref int gx, ref int gy, ref int gz);

        [DllImport("psmoveapi.dll")]
        public static extern void psmove_get_accelerometer(IntPtr move, ref int ax, ref int ay, ref int az);
        
        [DllImport("psmoveapi.dll")]
        public static extern void psmove_get_accelerometer_frame(IntPtr move, int frame, ref int ax, ref int ay, ref int az);

        [DllImport("psmoveapi.dll")]
        public static extern void psmove_enable_orientation(IntPtr move, bool enabled);

        [DllImport("psmoveapi.dll")]
        public static extern void psmove_reset_orientation(IntPtr move);

        [DllImport("psmoveapi.dll")]
        public static extern void psmove_get_orientation(IntPtr move, ref float w, ref float x, ref float y, ref float z);

        [DllImport("psmoveapi.dll")]
        public static extern bool psmove_has_calibration(IntPtr move);

        //tracker related
        [DllImport("psmoveapi_tracker.dll")]
        public static extern IntPtr psmove_tracker_new();

        [DllImport("psmoveapi_tracker.dll")]
        public static extern IntPtr psmove_tracker_new_with_camera(int cameraIndex);

        [DllImport("psmoveapi_tracker.dll")]
        public static extern bool psmove_tracker_enable(IntPtr tracker, IntPtr move);

        [DllImport("psmoveapi_tracker.dll")]
        public static extern int psmove_tracker_get_position(IntPtr tracker, IntPtr move, ref float x, ref float y, ref float radius);

        [DllImport("psmoveapi_tracker.dll")]
        public static extern float psmove_tracker_distance_from_radius(IntPtr tracker, float radius);

        [DllImport("psmoveapi_tracker.dll")]
        public static extern int psmove_tracker_update(ref IntPtr tracker, ref IntPtr move);

        [DllImport("psmoveapi_tracker.dll")]
        public static extern void psmove_tracker_update_image(ref IntPtr tracker);

        [DllImport("psmoveapi_tracker.dll")]
        public static extern int psmove_tracker_count_connected();

        [DllImport("psmoveapi_tracker.dll")]
        public static extern void psmove_tracker_free(ref IntPtr tracker);

        [DllImport("psmoveapi_tracker.dll")]
        public static extern int psmove_tracker_get_camera_color(ref IntPtr tracker, ref IntPtr move, ref byte r, ref byte g, ref byte b );



    }
}