using System.Globalization;
using Configs;
using Services.UiService;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Ui
{
    public class GameHudWindow : MonoBehaviour
    {
        [SerializeField] private Button _levelUpButton;
        [SerializeField] private TMP_Text _killCounter;
        [SerializeField] private TMP_Text _moveSpeed;
        [SerializeField] private TMP_Text _dps;
        [SerializeField] private TMP_Text _radius;
        
        private IUiProvider _uiProvider;

        public void Initialize(IUiProvider uiProvider, PlayerConfig playerConfig)
        {
            _uiProvider = uiProvider;
            
            _uiProvider.OnStatUp.Subscribe(UpdateStat).AddTo(this);
            _levelUpButton.OnClickAsObservable().Subscribe(_ => OnLevelUpClick()).AddTo(this);
            _uiProvider.OnKillCounterUpdate.Subscribe(UpdateKillCounter).AddTo(this);
            
            SetDefaultValues(playerConfig);
        }

        private void SetDefaultValues(PlayerConfig playerConfig)
        {
            _moveSpeed.text = playerConfig.MoveSpeed.ToString(CultureInfo.InvariantCulture);
            _dps.text = playerConfig.DamagePerSecond.ToString(CultureInfo.InvariantCulture);
            _radius.text = playerConfig.DamageRadius.ToString(CultureInfo.InvariantCulture);
        }

        private void UpdateStat(StatData statData)
        {
            switch (statData.stat)
            {
                case EStat.MoveSpeed:
                    _moveSpeed.text = statData.value.ToString(CultureInfo.InvariantCulture);
                    break;
                case EStat.DamagePerSecond:
                    _dps.text = statData.value.ToString(CultureInfo.InvariantCulture);
                    break;
                case EStat.AttackRadius:
                    _radius.text = statData.value.ToString(CultureInfo.InvariantCulture);
                    break;
            }
        }

        private void OnLevelUpClick()
        {
            _uiProvider.OnLevelUp();
        }
        
        private void UpdateKillCounter(int killCount)
        {
            _killCounter.text = killCount.ToString(CultureInfo.InvariantCulture);
        }
    }
}