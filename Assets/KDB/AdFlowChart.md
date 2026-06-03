# Ads Flow Chart

This document explains the main ad flow in the project and lists the active ad unit IDs. It is written for a general audience.

## Active Ad Network

- Primary ad network: **AdMob**
- Config source: `Assets/KDB/Scripts/GameAdsConfig.asset`
- Active ad types:
  - App Open Ad
  - Launch Interstitial
  - Normal Interstitial
  - Rewarded Video
  - Rewarded Interstitial
  - Banner
  - Secondary/backfill Interstitial and Reward (fallback use only)

## Active Ad Unit IDs

### AdMob app integration
- **AdMob App ID**: `ca-app-pub-3411062052281263~5705120266`

### AdMob ad units loaded from `GameAdsConfig.asset`
- **Launch Interstitial**: `ca-app-pub-3411062052281263/3169444505`
- **Interstitial**: `ca-app-pub-3411062052281263/3692188687`
- **Rewarded Video**: `ca-app-pub-3411062052281263/8038627800`
- **Rewarded Interstitial**: `ca-app-pub-3411062052281263/5294228744`
- **Secondary Interstitial**: `ca-app-pub-3411062052281263/2087800960`
- **Banner**: `ca-app-pub-3411062052281263/7601829325`
- **App Open Ad**: `ca-app-pub-3411062052281263/4441266716`
- **Secondary Reward**: `ca-app-pub-3411062052281263/4806664572`

### LevelPlay configuration (present but not active)
- **AppKey**: `1ab7561b5`
- **Launch Interstitial**: `68ozdvrgw2w8rw44`
- **Interstitial**: `z8axy0332hnr585z`
- **Rewarded**: `hgncqhneupu7bppt`

## Main Ad Flow

1. App starts and `AdManager` singleton is created.
2. `AdManager` reads `GameAdsConfig.asset` to load AdMob IDs.
3. AdMob SDK initializes and preloads:
   - App Open Ad
   - Launch Interstitial
   - Normal Interstitial
   - Rewarded Video
   - Rewarded Interstitial
   - Banner
4. On splash screen, the app may show:
   - **App Open Ad** first, if loaded and enabled
   - then **Launch Interstitial** before opening Main Menu
5. After the splash/menu transition, banners are shown on menu and gameplay screens.
6. Gameplay and UI events trigger more ads in these placements.
7. If primary AdMob loads fail, `SecondaryInterstitial` and `SecondaryReward` may be loaded as fallback IDs.

## Detailed Placement Map

### 1) App launch / first menu entry
- `AdManager.Start()` preloads App Open, Launch Interstitial, Interstitial, Rewarded, Rewarded Interstitial, and Banner.
- `splashtoMenu.cs` checks if the launch interstitial is ready and then calls `AdManager.ShowLaunchInterstitial(true)`.
- Result: yes, the first time the player enters the menu from splash, a **Launch Interstitial** is shown.
- If App Open was ready, it can also appear immediately on splash before the menu.

### 2) Main menu and world selection screens
- `WorldSelectionHandler.Start()` calls `AdManager.ShowbannerAd()`.
- This displays the **Banner** ad unit in menu/selection screens.
- `MainMenuScript.OnEnable()` hides banner briefly when the main menu appears.

### 3) Gameplay start
- `GameManager.Start()` calls `AdManager.ShowbannerAd()` for in-game banners.
- Banners are hidden during some popups and restored after the popup closes.

### 4) Level retry / “reload level” flow
- `GameManager.ReloadLevel()` shows a **Common Interstitial** by calling `AdManager.ShowCommonInterstitial()` when enough time has passed.
- This uses the normal **Interstitial** unit as the primary ad placement.

### 5) Return to level selection / back button
- `GameManager.GoBack()` also calls `AdManager.ShowCommonInterstitial()` before loading level selection.
- This is another normal **Interstitial** placement.

### 6) Level fail placement
- On failure, `GameManager` invokes `DelayShowGameFail()`, which checks if:
  - Enough time has passed since the last ad (`adDelayMet()`)
  - A rewarded ad is loaded (`adMobRewardedInterstitial.CanShowAd()`)
- If both conditions are true, the "Continue game?" panel appears, offering a rewarded ad to retry.
- If either condition is false, the normal level-fail screen is shown.
- This is where the **"Continue the Game"** reward flow is triggered.

### 6b) Continue the game via rewarded ad
- When player loses and "Continue game?" is shown, `rewardTypeToUnlock` is set to `RewardType.continuegame`.
- On ad click: `NeedExtralPanelButton.needextraballactivateStatic()` shows a **Rewarded Interstitial** (if available) or **Rewarded Video**.
- On ad success: `GameManager.ingamevideosuccess()` calls `donextlevelcall()` to load the next level (player's progress is preserved, but they move forward).
- On ad failure/skip: The player sees the level-fail screen again and cannot continue without spending coins or retrying manually.

### 7) Level complete placement
- `GameManager.DelayShowLevelCompleteAd()` chooses the next ad:
  - If **Launch Interstitial** has not yet been shown, it tries to show that first.
  - Otherwise it shows a game win **Interstitial**.
- So the end-of-level ad is either the **Launch Interstitial** or the normal **Interstitial**.

### 8) Rewarded ad placements
- `NeedExtralPanelButton` and `GameManager.NextLevel()` use `AdManager.ShowRewardedVideo()` for:
  - extra ball reward
  - continue game reward
  - skip level reward
- `StoreManager.watchRewardAd()` uses `AdManager.ShowRewardedVideo()` when the player needs coins in the store.
- `NotEnoughCoinsPopup.WatchAdToRetryLevel()` uses `AdManager.ShowRewardedVideo()` to retry a level.
- `SpinWheel.WatchVideoSpinIt()` uses `AdManager.ShowRewardedVideo()` for spin rewards.

### 9) Rewarded interstitial placement
- `NeedExtralPanelButton.needextraballactivateStatic()` may call `AdManager.ShowRewardedInterstitial()` when `RewardType.continuegame` is set.
- This uses the **Rewarded Interstitial** ad unit.

### 10) App foreground resume
- `AdManager.OnAppStateChanged()` can show **App Open Ad** when the app returns to foreground.
- If the user is not on the splash screen, the code opens a loading panel, then shows the App Open ad.

### 11) Secondary / fallback ad path
- `SecondaryInterstitial` and `SecondaryReward` IDs are only used if the primary AdMob load or request fails.
- `AdManager.CheckSecondaryInterstitialStatus(false)` loads `SecondaryInterstitial` when a reward-interstitial request fails and a fallback is configured.
- `AdManager.CheckSecondaryRewardAdStatus(false)` loads `SecondaryReward` if the reward video request fails.
- There is no direct `ShowSecondary...` placement; these IDs are applied by replacing the primary ID at runtime after a failure.

### 12) Custom provider fallback
- Several AdMob show methods also fall back to `CustomAdManager.Instance.ShowInterstitial()` when AdMob fails to show an ad.
- This means a secondary ad provider may be used if AdMob does not deliver.

### 13) Disabled / non-active placements
- `MainMenuScript.Exit()` calls `AdManager.ShowExitInterstitial()`, but this method immediately returns and is currently inactive.
- `showbannerExit()` is also stubbed out and does not show a real banner.
- LevelPlay code is present but not currently enabled for live display.

## Updated Ad Flow Diagram

```text
[App start]
    |
    +--> AdManager created
            |
            +--> Load GameAdsConfig.asset
            |       - AdMob App Open ID
            |       - AdMob Launch Interstitial ID
            |       - AdMob Interstitial ID
            |       - AdMob Rewarded Video ID
            |       - AdMob Rewarded Interstitial ID
            |       - AdMob Banner ID
            |       - Secondary Interstitial ID (fallback)
            |       - Secondary Reward ID (fallback)
            |
            +--> Load Global.InterstitialAdGap from server
            |       - minimum seconds between ads
            |
            +--> Initialize AdMob SDK
            |
            +--> Preload ads
                    - App Open
                    - Launch Interstitial
                    - Interstitial
                    - Rewarded Video
                    - Rewarded Interstitial
                    - Banner

Splash screen
    |
    +--> If App Open ready:
    |       show App Open Ad
    |
    +--> Then before Main Menu:
            show Launch Interstitial (if adDelayMet)

Main Menu / World Selection
    |
    +--> Show banner ad
    +--> Hide/show banner on main menu transitions

Gameplay
    |
    +--> Show banner ad on game start
    +--> Retry / reload level
    |       - Check: adDelayMet() AND InterstitialAdGap elapsed?
    |       - If YES: show common Interstitial
    |       - If NO: wait or skip
    +--> Back / level selection
    |       - Check: adDelayMet() AND InterstitialAdGap elapsed?
    |       - If YES: show common Interstitial
    +--> Level fail
    |       - Check: adDelayMet() AND rewarded ad loaded?
    |       - If YES: show "Continue game?" rewarded offer
    |       - If NO: show level fail screen
    |       |
    |       +--> "Continue game?" action:
    |               - Player watches Rewarded Interstitial or Rewarded Video
    |               - If success: continue to next level (with progress preserved)
    |               - If fail: back to level fail screen
    |
    +--> Level complete
            - If Launch Interstitial unused AND adDelayMet: show Launch Interstitial
            - Else if adDelayMet: show normal Game Win Interstitial
            - Else: skip to next level without ad

Rewarded ad flow (all types)
    |
    +--> Extra ball / revive
    |       - Shows Rewarded Video via ShowRewardedVideo
    |--> Continue game (after level fail)
    |       - Shows Rewarded Interstitial (preferred) or Rewarded Video
    |       - On success: skip to next level
    +--> Skip level
    |       - Shows Rewarded Video
    +--> Store coins
    |       - Shows Rewarded Video
    +--> Retry after not enough coins
    |       - Shows Rewarded Video
    +--> Spin wheel reward
            - Shows Rewarded Video

App foreground resume
    |
    +--> Check: adDelayMet() AND App Open ad available?
    |   - If YES: show App Open Ad via loading panel
    |   - If NO: resume without ad

Ad failure & fallback paths
    |
    +--> If primary interstitial load fails (after 60s+ gap):
    |       load SecondaryInterstitial ID as fallback
    |
    +--> If primary rewarded video load fails:
    |       load SecondaryReward ID as fallback
    |
    +--> If AdMob show fails:
            CustomAdManager.Instance.ShowInterstitial()

Disabled / inactive paths
    |
    +--> `MainMenuScript.Exit()` exit interstitial is inactive
    +--> `showbannerExit()` is stubbed and does not display a banner
    +--> LevelPlay config exists but is not active in live flow
```

## Updated Flow Chart (ASCII summary)

```
App start
  |
  +--> AdManager loads config + initializes AdMob
  |       |
  |       +--> Load Global.InterstitialAdGap from server (timing gate)
  |       |
  |       +--> Preloads App Open, Launch, Interstitial, Rewarded, Rewarded Interstitial, Banner
  |
  +--> Splash screen
  |       |
  |       +--> App Open Ad (if ready)
  |       |
  |       +--> Launch Interstitial before Main Menu
  |
  +--> Main Menu / world selection
  |       |
  |       +--> Show banner ad
  |
  +--> Gameplay
  |       |
  |       +--> Show banner ad
  |       +--> [Timing check] Retry / Reload => Common Interstitial (if InterstitialAdGap elapsed)
  |       +--> [Timing check] Back / Level selection => Common Interstitial (if InterstitialAdGap elapsed)
  |       +--> [Reward check] Level fail => "Continue game?" panel (if rewarded ad loaded)
  |       |       |
  |       |       +--> Watch ad:
  |       |               - Rewarded Interstitial (preferred)
  |       |               - OR Rewarded Video (fallback)
  |       |
  |       +--> [Timing check] Level complete => Launch or Game Win Interstitial (if InterstitialAdGap elapsed)
  |       +--> Rewarded offers (no timing gate) => Rewarded Video / Rewarded Interstitial
  |
  +--> App returns to foreground
  |       |
  |       +--> [Timing check] Show App Open Ad if available
  |
  +--> Ad fallback paths
          |
          +--> Primary load fails => try SecondaryInterstitial / SecondaryReward
          +--> AdMob show fails => CustomAdManager fallback
```

## What the chart means for a general audience

- The game uses one central ad controller: `AdManager`.
- AdMob is the live ad network, and it loads the ad unit IDs from `Assets/KDB/Scripts/GameAdsConfig.asset`.
- **Ad timing**: The game enforces a minimum delay (`Global.InterstitialAdGap`, typically 60+ seconds) between showing interstitials to prevent ad fatigue.
- The first major ad a user sees is usually a launch interstitial on the splash-to-menu transition (no delay on first ad).
- During gameplay, the app shows banners plus interstitials at retry, back, and level-complete moments (only if the delay has been met).
- **"Continue the game" feature**: When a player loses a level and a rewarded ad is available, they can choose to watch a brief ad to skip to the next level without losing progress. This uses **Rewarded Interstitial** ads when available, as they have higher engagement.
- Rewarded ads are intentionally placed where the player can choose a benefit: extra ball, continue, skip, store coins, retry, or spin (these have no delay gate).
- If AdMob cannot show an interstitial, the code may fall back to `CustomAdManager` or use a secondary ad unit.
- If a rewarded ad fails to load, a secondary reward ad unit may be attempted as a fallback.

## Ad Timing and Delay Mechanics

### Interstitial ad delay
- All interstitial placements are gated by `adDelayMet()`, which checks: `(DateTime.UtcNow - lastAdShownDateTime).TotalSeconds > Global.InterstitialAdGap`
- `Global.InterstitialAdGap` is set from server config (loaded at app start via Firebase).
- After any ad is shown, `lastAdShownDateTime` is updated, and no further interstitials can display until the delay is met.
- This prevents ad fatigue by enforcing a minimum time gap between consecutive ads.

### Level replay interval
- `levelReloadAdDuration = 60` seconds — reserved for level reload ad timing.
- `ReplayAdInterval = 60f` seconds — general replay/retry ad interval.

### Primary and secondary ad load failures
- If a primary interstitial or reward ad **fails to load**, the secondary fallback IDs are triggered:
  - **Primary Interstitial** fails → `AdManager.CheckSecondaryInterstitialStatus(false)` is invoked → `LoadSecondaryInterstitialAd()` replaces the primary ID with `SecondaryInterstitial` ID and retries.
  - **Primary Rewarded Video** fails → `AdManager.CheckSecondaryRewardAdStatus(false)` is invoked → `LoadSecondaryRewardAd()` replaces the primary ID with `SecondaryReward` ID and retries.
- These are **not direct show calls**; they are fallback load attempts after a primary load failure.

## "Continue the Game" Rewarded Ad Flow

### Overview
When a player loses a level and a rewarded ad is available, the app offers a "Continue game?" option. The player can watch an ad to retry the same level without losing progress.

### Detailed flow

1. **Level fail detected**: `GameManager.DelayShowGameFail()` is called.
   - `currentAdDisplayTime = Time.time`
   - `condition1`: Enough time since last ad (`adDelayMet()`)
   - `condition2`: Rewarded ad is loaded (`AdManager._instance.adMobNetworkHandler.adMobRewardedInterstitial.CanShowAd()`)

2. **Show "Continue game?" panel**:
   - If both conditions are true, set `AdManager._instance.rewardTypeToUnlock = RewardType.continuegame`.
   - Display `rewardCanvas` with the text "Continue game?" (localized as "Continuegame").

3. **Player clicks "Watch Ad"**:
   - `NeedExtralPanelButton.needextraballactivateStatic()` is called.
   - **If** `rewardTypeToUnlock == RewardType.continuegame`:
     - Show a **Rewarded Interstitial** ad via `AdManager.ShowRewardedInterstitial()`.
   - **Else**:
     - Show a regular **Rewarded Video** ad via `AdManager.ShowRewardedVideo()`.

4. **Ad completion**:
   - `GameManager.gameState = GameState.Reward_Video_Completed`
   - `AdManager._instance.rewardedvideosuccess` is set to `true` (if ad played successfully) or `false` (if skipped/failed).

5. **Reward applied via `ingamevideosuccess()`**:
   - This method is called by `OnApplicationFocus(true)` (app regains focus after ad).
   - It checks `rewardTypeToUnlock`:
     - If `continuegame` and `rewardedvideosuccess == true`:
       - Hide reward canvas
       - Set `rewardTypeToUnlock = RewardType.None`
       - Call `Invoke("donextlevelcall", 0.25f)` to proceed to the next level
     - If `continuegame` and `rewardedvideosuccess == false` (ad was skipped):
       - Hide reward canvas
       - The player loses and sees the level-fail screen again

6. **Next level loaded**: `donextlevelcall()` either increments level or world and loads the next scene.

### Other reward types
- **Extra ball** (`RewardType.extraball`): Rewarded video → adds one extra ball to retry the current level.
- **Skip level** (`RewardType.skiplevel`): Rewarded video → marks level as completed and moves to next level.
- **Store coins**: Rewarded video → grants in-game coins.
- **Retry after not enough coins**: Rewarded video → allows retry without spending coins.
- **Spin wheel**: Rewarded video → grants spin-wheel reward.

## Important Notes

- The app uses AdMob IDs listed earlier in this document.
- `MainMenuScript.Exit()` and `showbannerExit()` are not currently active ad paths.
- LevelPlay configuration exists in the project, but live playback uses AdMob first.
- Rewarded ad success is tracked through `rewardTypeToUnlock` and applied via `GameManager.ingamevideosuccess()`.
- The "continue the game" flow prioritizes **Rewarded Interstitial** ads when available, as they tend to have higher completion rates than regular rewarded videos.
