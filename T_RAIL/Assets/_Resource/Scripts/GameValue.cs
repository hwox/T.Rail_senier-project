

static class GameValue
{

    public enum itemCategory
    {
        food_tomato = 1, // 음식
        food_bean = 2,
        food_chicken = 3,
        hammer = 4, // 망치
        nail = 5, // 못
        medipack = 6,
        woodboard = 7,
        ironpan = 8 // 판(철판
        //  spanner = 7, // 스패너
    }

    public enum EnemyCategory
    {
        Rhino = 1,
        Cactus = 2,
        Husky = 3,
    }

    public enum prefab_list
    {
        bullet = 0,
        passenger = 1,
        stationpassenger = 2,
        dustparticle = 3,
        sofa = 4,
        box = 5,
        chicken = 6,
        egg = 7,
        coinparticle = 8,
           tomatosoup=9
    }
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
    public const int vandingmachine_layer = 27;
    public const int trainrepair_layer = 28;
    public const int sign_layer = 29;
    public const int statiopassenger_layer = 30;
    public const int wall_layer = 31;

    // 기차 영역에 구성되어있는 오브젝트들의 값
    // local 좌표임
    public const float furnitureX = 0.7f;
    public const float furnitureY = 0.35f;


    // 기차의 기본 체력
    public const int Train_Standard_HP = 100;
    // 기차 속성의 기본값 
    public const float Durability = 100.0f;
    public const float speed = 9.0f;
    public const int noise = 100;


    // 다음역 까지의 거리
    public const float NextStationMeter = 4000.0f; // 4km -> 기본속도 3기준 4분 // 4000인걸 1000으로 줄여놨음

    public const float TestMeter = 300.0f;

    // 기차 간격
    public const float Train_distance = -13.0f;

    // 기차의 기본 위치
    public const float Train_y = 3.0f;
    public const float Train_z = -2.0f;


    public const int MaxTrainNumber = 13;


    public const float player_2f_position_y = 7.6f;
    public const float player_1f_position_y = 3.8f;


    // enemy1 
    public const int enemy_FullHp = 200; // 한 텀

    // bullet
    public const float bullet_speed = 1000.0f;

    // status
    public const int StatusMAX = 5;

    // inTrainobject position
    public const float T_Box_ObjectY = 0.9f;

    public const float T_Sofa_ObjectY = 0.6f;
    public const float T_ObjectX = 1.3f;
    public static readonly float[] T_ObjectZ = new float[4] { 3.5f, 1.5f, -0.3f, -2.0f };

    //playerMaxHp
    public const int PlayerMaxHp = 10;


    // 아이템 사용
    public const int HungryDecrease = 30; // 음식먹으면
    public const int DiseaseDncrease = 30; // 약먹으면 

    public const int ITEMLIMIT = 6; // 한칸당 아이템 제한. 이름을 뭘로 해야될지 몰라서 LImit로


    public const int Stage1Index = 2; // stage1 끝이 몇번째 씬 인덱스인지 (역포함)
    public const int Stage2Index = 4; // (역포함)
    public const int Stage3Index = 6; //(엔딩씬포함)

    //
}


// 박스 제작에 필요한 물품





// 소파 제작에 필요한 물품