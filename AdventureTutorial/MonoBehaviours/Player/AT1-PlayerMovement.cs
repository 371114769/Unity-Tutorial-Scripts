using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour {

    public Animator animator;
    //启用AI
    public NavMeshAgent agent;
    public float inputHoldDelay = 0.5f;
    public float turnSpeedThreshold = 0.5f;
    public float speedDampTime = 0.1f;
    public float slowingSpeed = 0.175f;
    public float turnSmoothing = 15f;

    private WaitForSeconds inputHoldWait;
    //用来设置要移动过去的坐标
    private Vector3 destinationPosition;
    //Interactable是Class(Script)
    //点击目标必须有Interactable这个Script才能在Stopping这个方法里call一些变量
    private Interactable currentInteractable;
    //用来禁用鼠标操作
    private bool handleInput = true;

    //用const来保证变量不会被改变
    private const float stopDistanceProportion = 0.1f;
    private const float navMeshSampleDistance = 4f;

    //为什么要把animator的动画参数和Tag转换成int数字？
    //因为animator.SetFloat的参数里要用到int ID
    private readonly int hashSpeedPara = Animator.StringToHash("Speed");
    private readonly int hashLocomotionTag = Animator.StringToHash("Locomotion");

    private void Start()
    {
        //这里不用agent的rotation而是自己设置rotation
        //如果只用agent.updateRotation会造成人物转身困难，人物只会像TankTutorial那样慢慢旋转，但直行的速度不变
        //结果是用鼠标点击人物后边的话，人物会变往前走，边匀速Y旋转，得饶一个大圈才回得来
        agent.updateRotation = false;

        //在交互的时候会冻结操作半秒钟，
        inputHoldWait = new WaitForSeconds(inputHoldDelay);

        //在开场不要让角色移动
        destinationPosition = transform.position;
    }

    private void OnAnimatorMove()
    {
        //根据上一帧的avatar的移动距离在设置速度
        //animator.deltaPosition好像是上一个渲染帧的vector3移动的距离(增量)
        //所以1帧移动的量除以1帧的时间的到单位时间的移动速度
        //我猜是为了让动画移动的更平滑，防止滑步漂移
        agent.velocity = animator.deltaPosition / Time.deltaTime;
    }

    private void Update()
    {
        //鼠标点击以后agent会计算移动路径，等到计算完路径以后再进行移动。
        if (agent.pathPending)
            return;

        //把desiredVelocity转换成float Type
        float speed = agent.desiredVelocity.magnitude;

        //stoppingDistance是在Player对象的NavMeshAgent里设置了0.15
        //距离小于0.15的时候，停用agent然后用自己的代码让玩家减速前进
        //距离小于0.15*stopDistanceProportion的时候，停用agent让玩家停下来
        if (agent.remainingDistance <= agent.stoppingDistance * stopDistanceProportion)
            Stopping(out speed);

        //距离小于0.15的时候就不用agent了，而是用自己的代码让Player走动
        else if (agent.remainingDistance <= agent.stoppingDistance)
            Slowing(out speed, agent.remainingDistance);

        //速度大雨turnSpeedThreshold=0.5f的时候才开始执行Move，有了这个速度才让角色瞬间转身
        else if (speed > turnSpeedThreshold)
            Moving();

        //设置人物动画的Speed值（speedDampTime用来平滑speed下降速度
        animator.SetFloat(hashSpeedPara, speed, speedDampTime, Time.deltaTime);
    }

    void Stopping(out float speed)
    {
        //停用agent的移动
        agent.isStopped = true;
        //直接把人物位置搬到鼠标点击位置
        transform.position = destinationPosition;
        //把速度设置为0，确保人物不会执行走路的动画
        speed = 0f;

        //这个到底是怎么联系上可交互的东西呢？
        if(currentInteractable)
        {
            //获得这个可交互的物件，然后定位这个物件的script，然后读取特定的component
            transform.rotation = currentInteractable.interactionLocation.rotation;
            //进行交互行为，也就是Call Interactable Script的Interact()方法
            currentInteractable.Interact();
            //交互后把currentInteractable设置为无，防止无限交互
            currentInteractable = null;
            //禁用鼠标操作，等个半秒，或者拾取动作完成，后再启用鼠标的移动操作
            StartCoroutine(WaitForInteraction());
        }
    }

    void Slowing(out float speed, float distanceToDestination)
    {
        //停用agent的移动
        agent.isStopped = true;
        //让角色立即对准destinationPosition的位置，并且以slowingSpeed前进
        transform.position = Vector3.MoveTowards(transform.position, destinationPosition, slowingSpeed * Time.deltaTime);

        //distanceToDestination / agent.stoppingDistance随着移动占比越来越小
        //...然后用1f减去上面的值，也就是说数值越来越接近1
        //...Lerp把速度降到0f的所需时间越来越短，所以动画速度随着距离变化越来越慢
        float proportionalDistance = 1f - distanceToDestination / agent.stoppingDistance;
        speed = Mathf.Lerp(slowingSpeed, 0f, proportionalDistance);

        //如果点击的对象是个Interactable，则使用特定的旋转方向
        //...然后也是旋转速度由快到慢，取决于距离，和动画速度成正比
        Quaternion targetRotation = currentInteractable ? currentInteractable.interactionLocation.rotation : transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, proportionalDistance);
    }

    void Moving()
    {
        //agent.desiredVelocity里面有本地的向量，它的方向用于旋转的朝向
        Quaternion targetRotation = Quaternion.LookRotation(agent.desiredVelocity);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
    }


    //在这里用PointerEventDat的时候，SecurityRoom对象的EventTrigger组件里就没的选用OnGroundClick
    //...然后点击地面就无法移动了，为什么？
    public void OnGroundClick(BaseEventData data)
    {
        //交互的会设置成false一段时间，防止鼠标操作移动
        if (!handleInput)
            return;
        //如果点击的是地面，确保不会在Slowing()和Stopping()中Call一些交互有关的变量
        currentInteractable = null;

        PointerEventData pData = (PointerEventData)data;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(pData.pointerCurrentRaycast.worldPosition, out hit, navMeshSampleDistance, NavMesh.AllAreas))
        {
            destinationPosition = hit.position;
        }
        else
        {
            destinationPosition = pData.pointerCurrentRaycast.worldPosition;
        }

        agent.SetDestination(destinationPosition);
        agent.isStopped = false;
    }

    public void OninteractableClick(Interactable interactable)
    {
        //同上，相当于禁用鼠标点击的功能
        if (!handleInput)
            return;

        //引用可交互的变量
        currentInteractable = interactable;
        //把要移动的地点设置成这个交互物件特定的地方
        //获得这个可交互的物件，然后定位这个物件的script，然后读取特定的component，这里的interactionLocation也就是Transform Type
        destinationPosition = currentInteractable.interactionLocation.position;

        agent.SetDestination(destinationPosition);
        agent.isStopped = false;
    }


    IEnumerator WaitForInteraction()
    {
        //禁用鼠标操作
        handleInput = false;

        //...等半秒钟先
        yield return inputHoldWait;

        //...然后看看当前的交互动作有没有locomotion的Tag(也就是移动和停止的动作)
        //作用是在执行拿东西的动作时，禁用鼠标操作，防止拿着东西就漂移了
        while(animator.GetCurrentAnimatorStateInfo(0).tagHash != hashLocomotionTag)
        {
            yield return null;
        }

        //最后再允许鼠标操作
        handleInput = true;
    }

}
