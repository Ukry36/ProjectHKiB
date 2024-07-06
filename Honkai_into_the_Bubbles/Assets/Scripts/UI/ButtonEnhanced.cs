using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/ButtonEnhanced")]
    public class ButtonEnhanced :
    Selectable, IPointerEnterHandler, ISelectHandler, IPointerClickHandler, ISubmitHandler, IPointerExitHandler, IDeselectHandler
    {
        [Serializable]
        public class ButtonSelectedEvent : UnityEvent { }
        [FormerlySerializedAs("onSelect")]
        [SerializeField]
        private ButtonSelectedEvent m_onSelect = new();

        [Serializable]
        public class ButtonClickedEvent : UnityEvent { }
        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickedEvent m_OnClick = new();

        [Serializable]
        public class ButtonDeselectedEvent : UnityEvent { }
        [FormerlySerializedAs("onDeselect")]
        [SerializeField]
        private ButtonDeselectedEvent m_onDeselect = new();

        protected ButtonEnhanced()
        { }

        public ButtonSelectedEvent onSelect
        {
            get { return m_onSelect; }
            set { m_onSelect = value; }
        }

        public ButtonClickedEvent onClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        public ButtonDeselectedEvent onDeselect
        {
            get { return m_onDeselect; }
            set { m_onDeselect = value; }
        }

        private void LayOn()
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("Button.onSelect", this);
            m_onSelect.Invoke();
        }
        private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("Button.onClick", this);
            m_OnClick.Invoke();
        }
        private void LayOff()
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("Button.onDeselect", this);
            m_onDeselect.Invoke();
        }


        public override void OnPointerEnter(PointerEventData eventData)
        {
            LayOn();
            DoStateTransition(SelectionState.Highlighted, false);
        }
        public override void OnSelect(BaseEventData eventData)
        {
            LayOn();
            DoStateTransition(SelectionState.Selected, true);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Press();
            DoStateTransition(SelectionState.Pressed, true);
        }
        public virtual void OnSubmit(BaseEventData eventData)
        {
            Press();
            if (!IsActive() || !IsInteractable())
                return;

            DoStateTransition(SelectionState.Pressed, true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            LayOff();
            DoStateTransition(SelectionState.Normal, true);
        }
        public override void OnDeselect(BaseEventData eventData)
        {
            LayOff();
            DoStateTransition(SelectionState.Normal, true);
        }
    }
}
