

static class GameValue {


    // 카메라 셋팅
    public const float Mcam_initrot_x = 20.0f;
    public const int Mcam_initFOV = 60;

    public const float Mcam_changerot_x = 10.0f;
    public const int Mcam_changeFOV = 70;


    // layer int값
    public const int itembox_layer = 12;
    public const int passenger_layer = 13;
    public const int choice_layer = 14;
    public const int ladder_layer = 15;
    public const int machinegun_layer = 16;
    public const int sofa_layer = 17;
    public const int NextTrain_layer = 18;
    public const int PrevTrain_layer = 19;
    public const int floor2_layer = 20;
    public const int player_layer = 21;
    public const int bullet_layer = 22;
    public const int enemy_layer = 23;
    public const int train_layer = 24;
    public const int chicken_layer = 25;
    public const int egg_layer = 26;
    public const int sign_layer = 29;
    public const int statiopassenger_layer = 30;
    public const int wall_layer = 31;

    // 기차 영역에 구성되어있는 오브젝트들의 값
    // local 좌표임
    public const float furnitureX = 0.7f;
    public const float furnitureY = 0.35f;


    // 기차의 기본 체력
    public const int Train_Standard_HP = 300;
    // 기차 속성의 기본값 
    public const float Durability = 100.0f;
    public const float speed = 9.0f;
    public const int noise = 100;


    // 다음역 까지의 거리
    //public const float NextStationMeter = 4000.0f; // 4km -> 기본속도 3기준 4분 // 4000인걸 1000으로 줄여놨음
    public const float NextStationMeter = 2000.0f; // 4km -> 기본속도 3기준 4분 // 4000인걸 1000으로 줄여놨음

    // 기차 간격
    public const float Train_distance = -13.0f;

    // 기차의 기본 위치
    public const float Train_y = 3.0f;
    public const float Train_z = -2.0f;


    public const int MaxTrainNumber = 13;


    public const float player_2f_position_y = 7.6f;
    public const float player_1f_position_y = 3.8f;


    // enemy1 
    public const int enemy1_FullHp = 200; // 한 텀


    // bullet
    public const float bullet_speed = 1000.0f; // 미친 처음에 총알 속력 50? 막 이렇게 했는데 ㅋㅋㅋㅋ600m/s말도안돼

    // status
    public const int StatusMAX = 5;

    // inTrainobject position
    public const float T_Box_ObjectY = 0.9f;

    public const float T_Sofa_ObjectY = 0.6f;
    public const float T_ObjectX = 1.3f;
    public static readonly float[] T_ObjectZ = new float[4] { 3.5f, 1.5f, -0.3f, -2.0f };

    //playerMaxHp
    public const int PlayerMaxHp = 10;




}
