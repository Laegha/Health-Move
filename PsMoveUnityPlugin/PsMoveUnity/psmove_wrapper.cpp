#include "psmove.h"
#include "psmoveapi.h"
#include "psmove_tracker.h"

#include <stdio.h>
#include "pch.h"

extern "C" {

    __declspec(dllexport) void InitializePSMove() {
        if (psmove_init(PSMOVE_CURRENT_VERSION) == 0) {
            printf("Could not initialize PSMove API\n");
        }
    }

    __declspec(dllexport) PSMove* ConnectControllerByID(int id) {
        return psmove_connect_by_id(id);
        
    }

    __declspec(dllexport) void DisconnectController(PSMove* move) {
        psmove_disconnect(move);
    }

    __declspec(dllexport) int GetCountConnected() {
        return psmove_count_connected();
    }

    __declspec(dllexport) int ControllerHasChangedState(PSMove *move) {
        return psmove_poll(move);
    }

    __declspec(dllexport) int* GetAcceleration(PSMove* move, int x, int y, int z) {
        int ax = x;
        int ay = y;
        int az = z;
        psmove_tracker_get_position();
        psmove_tracker_enable
        psmove_tracker_update();
        psmove_tracker_update_image();
        psmove_tracker_distance_from_radius();

        psmove_get_accelerometer_frame(move, &ax, &ay, &az);

        int* returnedValues = new int[3];
        returnedValues[0] = ax;
        returnedValues[1] = ay;
        returnedValues[2] = az;

        return returnedValues;
    }

    __declspec(dllexport) int* GetGyro(PSMove* move, int x, int y, int z) {
        int ax = x;
        int ay = y;
        int az = z;
        psmove_get_gyroscope(move, &ax, &ay, &az);
        psmove_

        int* returnedValues = new int[3];
        returnedValues[0] = ax;
        returnedValues[1] = ay;
        returnedValues[2] = az;

        return returnedValues;
    }

    __declspec(dllexport) void SetLeds(PSMove* move, int r, int g, int b) {
        psmove_set_leds(move, r, g, b);
    }

    __declspec(dllexport) void PairController(PSMove* move) {
        psmove_pair(move);
    }

    __declspec(dllexport) int GetBattery(PSMove* move) {
        return psmove_get_battery(move);
    }

    __declspec(dllexport) unsigned int GetButtons(PSMove* move) {
        return psmove_get_buttons(move);
    }
}