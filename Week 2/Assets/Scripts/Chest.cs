using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour
{
    public InputActionAsset inputActionAsset;
    private InputAction openAction;
    private InputAction closeAction;

    [SerializeField] public InventoryUI playerInventory;
    [SerializeField] public InventoryUI chestInventory;

    private PlayerInput playerInput;
    
    private bool playerNearby;
    private bool isOpen;

    // Start is called before the first frame update
    private void Start()
    {   
        //trys to find an input action
        openAction = inputActionAsset.FindAction("Interact");
        closeAction = inputActionAsset.FindAction("Exit");
        
        //checks if open action exists
        if (openAction == null)
        {
            GameLogger.LogWarning("Open Action was unable to be found");
        }
        
        //checks if close action exists
        if (closeAction == null)
        {
            GameLogger.LogWarning("Close Action was unable to be found");
        }
        
        //finds the player
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            //assigns the input from the player
            playerInput = player.GetComponent<PlayerInput>();
            if (!playerInput)
            {
                GameLogger.LogWarning("Player Input was unable to be found");
            }
        }
        else
        {
            GameLogger.LogWarning("Player was unable to be found");
        }
        
        //checks if player inventory exists
        if (!playerInventory)
        {
            GameLogger.LogWarning("Player Inventory not found");
        }
        
        //checks if chest inventory exits
        if (!chestInventory)
        {
            GameLogger.LogWarning("Chest Inventory not found");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        TryToOpen();
    }

    private void TryToOpen()
    {

        //if the player isn't nearby, don't do anything
        if (!playerNearby) return;

        if (isOpen)
        {
            //see if the close action button was pressed
            if (closeAction.WasPressedThisFrame())
            {
                //closes the chest
                OnClose();
            }
            
        }
        else
        {
            //see if the open action button was pressed
            if (openAction.WasPressedThisFrame())
            {
                //opens chest
                OnOpen();
            }
        }

    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public void OnOpen()
    {
        if (playerInput)
        {
            playerInput.SwitchCurrentActionMap("UI");
        }
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerInventory.gameObject.SetActive(true);
        chestInventory.gameObject.SetActive(true);

        //flips isOpen
        isOpen = !isOpen;
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public void OnClose()
    {
        if (playerInput)
        {
            //give control back to the player
            playerInput.SwitchCurrentActionMap("Player");
        }

        InfoBox infoBox = FindObjectOfType<InfoBox>();
        infoBox.Vanish();
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerInventory.gameObject.SetActive(false);
        chestInventory.gameObject.SetActive(false);

        //flips isOpen
        isOpen = !isOpen;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}

