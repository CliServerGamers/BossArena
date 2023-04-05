using BossArena.game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BossArena.UI
{

    public class HealthBarUI : UIPanelBase
    {
        [field: SerializeField]
        private EntityBase entity;
        public Image currentHealthBar;
        public Text healthText;

        void HealthChanged(float oldHealth, float currHealth)
        {
            float ratio = currHealth / entity.MaxHealth.Value;
            currentHealthBar.rectTransform.localPosition = new Vector3(currentHealthBar.rectTransform.rect.width * ratio - currentHealthBar.rectTransform.rect.width, 0, 0);
            healthText.text = currHealth.ToString("0") + "/" + entity.MaxHealth.Value.ToString("0");
        }
        public override void Start()
        {
            base.Start();
            StartCoroutine(InitHealthBar());
           
        }
        IEnumerator InitHealthBar()
        {
            if (entity is null)
            {
                yield return StartCoroutine(WaitForLocalPlayer());
            }
            HealthChanged(0f, entity.CurrentHealth.Value);
            entity.CurrentHealth.OnValueChanged += HealthChanged;
        }
        IEnumerator WaitForLocalPlayer()
        {
            if (InGameRunner.Instance.LocalPlayer == null)
                yield return null;
            entity = InGameRunner.Instance.LocalPlayer.GetComponent<EntityBase>();
        }
    }

}