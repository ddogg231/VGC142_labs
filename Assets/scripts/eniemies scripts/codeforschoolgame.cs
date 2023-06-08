using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class codeforschoolgame : MonoBehaviour
{
   
}
public class CustomerQueueManger : MonoBehaviour
{
    private Queue<CustomerClass1> customerQueue = new Queue<CustomerClass1>();

    [SerializeField]
    private List<Vector3> waitingQueuePostionList = new List<Vector3>();

    // Start is called before the first frame update
    private void Start()
    {


        //where the firstposition is located in scene
        Vector3 firstposition = new Vector3(-2.5f, 0f);
        float positionSize = 9f;
        for (int i = 0; i < 5; i++)
        {
            waitingQueuePostionList.Add(firstposition + new Vector3(-1, 0) * positionSize);
        }

    }

    public void AddCustomerToQueue(CustomerClass1 customer)
    {
        customerQueue.Enqueue(customer);
        Debug.Log("Customer added to the queue: " + customer.name);

        // Set customer position in the waiting queue
        if (customerQueue.Count <= waitingQueuePostionList.Count)
        {
            int index = customerQueue.Count - 1;
            customer.agent.SetDestination(waitingQueuePostionList[index]);
        }
        else
        {
            Debug.LogWarning("Not enough positions in the waiting queue for customer: " + customer.name);
        }
    }
    public void ServeNextCustomer()
    {
        if (customerQueue.Count > 0)
        {
            CustomerClass1 nextCustomer = customerQueue.Dequeue();
            Debug.Log("Serving customer: " + nextCustomer.name);

            // Move the remaining customers in the waiting queue forward
            int index = 0;
            foreach (CustomerClass1 customer in customerQueue)
            {
                customer.agent.SetDestination(waitingQueuePostionList[index]);
                index++;
            }
        }
        else
        {
            Debug.LogWarning("No customers in the queue to serve.");
        }
    }
}

public class waitingQueue : MonoBehaviour
{
    private List<Vector3> positionList;
    private List<CustomerClass1> customerList;
    private Vector3 entrancePosition;
    public waitingQueue(List<Vector3> positionList)
    {
        this.positionList = positionList;
        entrancePosition = positionList[positionList.Count - 1] + new Vector3(-8, 0);

        customerList = new List<CustomerClass1>();
    }

    public bool CanAddGuest()
    {
        return customerList.Count < positionList.Count;
    }

    public void AddGuest(CustomerClass1 customer)
    {
        customerList.Add(customer);
        int index = customerList.IndexOf(customer);

        if (index >= 0 && index < positionList.Count)
        {
            customer.agent.SetDestination(positionList[index]);
        }
        else
        {
            Debug.LogError("Invalid position index for customer!");
        }
    }

    public CustomerClass1 GetFirstInQueue()
    {
        if (customerList.Count == 0)
        {
            return null;
        }
        else
        {
            CustomerClass1 firstCustomer = customerList[0];
            customerList.RemoveAt(0);
            return firstCustomer;
        }
    }
}

public class CustomerClass1 : CustomerBase
{
    public enum CustomerState
    {
        //add movetosit when sits are available
        InLine, Ordering, Waiting, Leaving
    }

    //Initial State
    public CustomerState currentState = CustomerState.InLine;

    //Waiting In line to order Array
    public GameObject[] Line;
    public int LineIndex;
    //Walking around waiting for coffee Array
    public GameObject[] Waypoint;
    public int WaypointIndex;
    public static waitingQueue waitingQueue;

    /// <summary>
    ///  For the arrays im thinking of moving them to some other script like a level script or something to be actually be 
    ///  accesible by all customer prefabs on what Index they currently are in 
    ///  (im thinking of copying something like the queue DT with had from programming fundi to the line array)
    ///  we can also add sits as an array instead of waypoints
    /// </summary>

    private CustomerQueueManger queueManager;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        queueManager = FindObjectOfType<CustomerQueueManger>();

        //Setting Arrays - Subject to change, how to handle multiple customers
        if (Line.Length <= 0) Line = GameObject.FindGameObjectsWithTag("Line");
        if (Waypoint.Length <= 0) Waypoint = GameObject.FindGameObjectsWithTag("Waypoint");

        //Initial State
        if (currentState == CustomerState.InLine)
        {
            target = Line[LineIndex].transform;
            if (target) agent.SetDestination(target.position);
        }


    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (currentState == CustomerState.Ordering)
        {
            Order();

            //add the interact thing here when player interacts with player
            //for now im just gonna put a delay
            Invoke("OrderTaken", 10f);

        }

        if (currentState == CustomerState.Waiting)
        {
            if (agent.remainingDistance < distThreshold)
            {
                WaypointIndex++;
                WaypointIndex %= Waypoint.Length;
                target = Waypoint[WaypointIndex].transform;

                if (target) agent.SetDestination(target.position);
            }

        }

        if (currentState == CustomerState.InLine)
        {
            if (agent.remainingDistance < distThreshold) currentState = CustomerState.Ordering;
        }

        Debug.Log("The customer is " + currentState + " and is going to " + target.gameObject);
    }

    public virtual void OrderTaken()
    {
        currentState = CustomerState.Waiting;
        target = Waypoint[WaypointIndex].transform;

        if (target) agent.SetDestination(target.position);

        //start timer
        //spawn UI for things needed
        //something about recording and comparing if order is correct
        queueManager.ServeNextCustomer();
    }

    public virtual void Order()
    {
        //UI - customer waiting for Player to hear order
        //Order
        //some other timer? them we could puit that in an invoke then make it leave
        queueManager.AddCustomerToQueue(this);
    }

}

[RequireComponent(typeof(NavMeshAgent))]
public class CustomerBase : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;

    public float distThreshold;


    /// <summary> Suggested stuff in common
    /// timer for time to deal with? we can have a universal starting one then we can override it after in like different character classes
    /// name? for if we want to pursue name writing in cup and calling, or just to have it here in base class
    /// 
    /// </summary>



    // Start is called before the first frame update
    public virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();


        if (distThreshold <= 0) distThreshold = 0.5f;

        //we can add the randomization of meshes or skins here then add more stuff in specific classes?

    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!target) return;





    }

    public virtual void CustomerLeave()
    {

    }


}
