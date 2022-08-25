using UnityEngine;
using UnityEngine.InputSystem;

public class ScriptingLesson : MonoBehaviour
{
    // Data and Variables
    public bool isActive;
    public int MyInt;
    public float MyFloat = 3.14f;
    public string MyString = "Myvari";
    public GameObject MyGameObject;
    public int AddA;
    public int AddB;
    public int TotalAdd;
    private InputAction TestInput = new InputAction("test", binding: "<Keyboard>/b");


    // Logic and Flow
    private void Start()
    {
        isActive = true;
        MyInt = 10;
    }

    // OnEnable and OnDisable have to be done per action from the input system to wake them up for use
    private void OnEnable()
    {
        // Add a listener (subscribe) to be notified when this action is performed
        TestInput.performed += OnTestInput;
        TestInput.Enable();
    }

    private void OnDisable()
    {
        // Always remember to remove your listener (unsubscribe) when no longer needed, otherwise sometimes
        // undesirable and unknown side effects can occur
        TestInput.performed -= OnTestInput;
        TestInput.Disable();
    }

    private void Update()
    {
        if (MyGameObject != null)
        {
            if (isActive)
            {
                MyGameObject.SetActive(isActive);
            }
            else
            {
                MyGameObject.SetActive(isActive);
            }
        }

        while (MyInt > 0)
        {
            // Debug.Log($"MyInt should be less than zero. It's currently at: {MyInt}");
            MyInt--;
        }

        for (int i = 0; i < 10; i++)
        {
            // Debug.Log($"For Loop number: {i}");
        }
    }

    // Functions
    private int IntAdd(int a, int b)
    {
        TotalAdd = a + b;
        return TotalAdd;
    }

    // This is automatically called when the user performs any input triggering the action
    private void OnTestInput(InputAction.CallbackContext actionContext)
    {
        // If the action was performed (pressed) this frame
        if (actionContext.performed)
        {
            Debug.Log(IntAdd(AddA, AddB));
        }
    }
}

