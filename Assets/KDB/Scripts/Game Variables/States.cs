using UnityEngine;
using System.Collections;

public enum SlingshotState {
	Idle,
	UserPulling,
	BirdFlying
}

public enum GameState {
	Start,
	BirdMovingToSlingshot,
	Playing,
    Pause,
	Won,
	Lost,
    RewardState_Success,
    Reward_Video_Started,
    Reward_Video_Completed,

}

public enum BirdState {
	BeforeThrown,
	Thrown
}