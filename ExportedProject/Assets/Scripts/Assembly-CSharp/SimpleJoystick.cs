using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class SimpleJoystick : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IDragHandler
{
	public enum AxisOption
	{
		Both = 0,
		OnlyHorizontal = 1,
		OnlyVertical = 2
	}

	public int MovementRange;

	public AxisOption axesToUse;

	public string horizontalAxisName;

	public string verticalAxisName;

	private Vector3 m_StartPos;

	private bool m_UseX;

	private bool m_UseY;

	private CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis;

	private CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis;

	public bool m_UseGrid;

	private float m_GridCount;

	public bool m_UseCircle;

	private void InitJoystick()
	{
	}

	private void OnEnable()
	{
	}

	private void Start()
	{
	}

	private void UpdateVirtualAxes(Vector3 value)
	{
	}

	private void CreateVirtualAxes()
	{
	}

	public void OnDrag(PointerEventData data)
	{
	}

	public void OnPointerUp(PointerEventData data)
	{
	}

	public void OnPointerDown(PointerEventData data)
	{
	}

	private void OnDisable()
	{
	}
}
