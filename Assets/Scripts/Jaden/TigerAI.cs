using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class TigerAI : MonoBehaviour
{
    // Tiger States
    public enum TigerState
    {
        Idle,
        HuntingSearching,
        Prowling,
        Stalking,
        Evacuating,
        Chase
    }

    [Header("State Management")]
    public TigerState currentState = TigerState.Idle;
    private TigerState previousState = TigerState.Idle; // Track previous state

    [Header("Hidden Stats")]
    public float aggressiveness = 0f;
    public float fullAgressiveness = 0f; //The total with awareness calculated.
    public float awareness = 0f;
    public float playerDistance = 0f;
    public bool isPlayerLooking = false;

    [Header("Timers")]
    private float idleTimer = 0f; //How long it takes to get out of idle
    private float lowAwarenessTimer = 0f; //How long it takes to get into idle
    private float stateTransitionTimer = 0f;
    private bool hasSetProwlingDestination = false;
    private bool hasSetStalkingDestination = false;
    private bool hasSetEvacuatingDestination = false;

    [Header("StateModifiers")]
    private bool prowlChase = false;
    private bool stalkChase = false;
    public bool isCheckingLook = false;
    public bool isCheckingAware = false;
    public bool isAttacking = false;

    [Header("References")]
    public NavMeshAgent navMeshAgent;
    private Transform player;
    private GameManager gameManager;
    private Animator animator;

    [Header("State Parameters")]
    [SerializeField] private float maxAggressiveness = 20f;
    [SerializeField] private float idleToActiveDelay = 1f;
    [SerializeField] private float lowAwarenessThreshold = 1f;
    [SerializeField] private float lowAwarenessDurationForIdle = 10f;

    [Header("Teleport Settings")]
    [SerializeField] private float teleportDistanceThreshold = 200f;
    [SerializeField] private float teleportMinDistance = 80f;
    [SerializeField] private float teleportMaxDistance = 120f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponentInChildren<Animator>();

        InitializeTiger();
    }

    void Update()
    {
        UpdateHiddenStats();
        UpdatePlayerDistance();
        isPlayerLooking = IsPlayerLookingAtTiger();

        // Check if tiger is too far from player and teleport if needed
        if (playerDistance > teleportDistanceThreshold)
        {
            TeleportToRandomLocationNearPlayer();
            TransitionToState(TigerState.HuntingSearching);
        }

        if (currentState != previousState)
        {
            // Reset flags when state changes
            hasSetProwlingDestination = false;
            hasSetStalkingDestination = false;
            hasSetEvacuatingDestination = false;

            previousState = currentState;
        }

        switch (currentState)
        {
            case TigerState.Idle:
                HandleIdleState();
                break;
            case TigerState.HuntingSearching:
                HandleHuntingSearchingState();
                break;
            case TigerState.Prowling:
                HandleProwlingState();
                break;
            case TigerState.Stalking:
                HandleStalkingState();
                break;
            case TigerState.Evacuating:
                HandleEvacuatingState();
                break;
            case TigerState.Chase:
                HandleChaseState();
                break;
        }

        CheckStateTransitions();
    }

    private void InitializeTiger()
    {
        // Initialize based on day count
        aggressiveness += Mathf.Clamp((gameManager.currentDay * 2) - 1, 1, 13);
        aggressiveness = Mathf.Clamp(aggressiveness, 0, maxAggressiveness);
    }

    private void UpdateHiddenStats()
    {
        // Update awareness based on player activity (to be implemented)
        // Update aggressiveness based on awareness level and bodies found
        UpdateAggressiveness();
    }

    private void UpdateAggressiveness()
    {
        // Apply awareness modifiers
        switch (Mathf.FloorToInt(awareness))
        {
            case 0: fullAgressiveness = aggressiveness - 2; break;
            case 1: fullAgressiveness = aggressiveness; break;// No change
            case 2: fullAgressiveness = aggressiveness + 1; break;
            case 3: fullAgressiveness = aggressiveness + 3; break;
        }

        // Apply body count modifiers (to be implemented)
        // aggressiveness += gameManager.bodiesFound;

        fullAgressiveness = Mathf.Clamp(fullAgressiveness, 0, maxAggressiveness);
    }

    private void UpdatePlayerDistance()
    {
        if (player != null)
        {
            playerDistance = Vector3.Distance(transform.position, player.position);
        }
        else
        {
            // Try to find player again if reference was lost
            print("lost");
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void CheckStateTransitions()
    {
        // Check for idle condition
        if (awareness < lowAwarenessThreshold &&
            currentState != TigerState.Stalking &&
            currentState != TigerState.Prowling)
        {
            lowAwarenessTimer += Time.deltaTime;
            if (lowAwarenessTimer >= lowAwarenessDurationForIdle && currentState != TigerState.Idle)
            {
                TransitionToState(TigerState.Idle);
            }
        }
        else
        {
            lowAwarenessTimer = 0f;
        }
    }

    #region State Handlers
    private void HandleIdleState()
    {
        // Stop movement
        navMeshAgent.isStopped = true;

        // Check if awareness increases to exit idle
        if (awareness >= lowAwarenessThreshold)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleToActiveDelay)
            {
                TransitionToState(TigerState.HuntingSearching);
                idleTimer = 0f;
            }
        }
        else
        {
            idleTimer = 0f;
        }
    }

    private void HandleHuntingSearchingState()
    {
        navMeshAgent.isStopped = false;

        // Higher aggressiveness increases speed
        navMeshAgent.speed = CalculateHuntingSpeed();

        // Search behavior - wander or move toward last known position
        if (!navMeshAgent.hasPath || navMeshAgent.remainingDistance < 1f)
        {
            if (player != null)
            {
                // Calculate direction to player with random angle offset
                Vector3 directionToPlayer = (player.position - transform.position).normalized;

                // Make angle range tighter based on aggressiveness (0-20 range)
                // At max aggressiveness (20): range is -15 to 15 (tight focus)
                // At min aggressiveness (0): range is -70 to 70 (wide wandering)
                float aggressivenessFactor = Mathf.Clamp01((float)fullAgressiveness / 20f); // 0 to 1
                float maxAngle = Mathf.Lerp(70f, 15f, aggressivenessFactor); // Lerp from 70 to 15

                int randomAngle = Random.Range(-(int)maxAngle, (int)maxAngle);
                Quaternion randomRotation = Quaternion.Euler(0, randomAngle, 0);
                Vector3 randomDirection = randomRotation * directionToPlayer;

                // Calculate a point in that direction
                Vector3 targetPosition = transform.position + randomDirection * 20f;

                // Find the nearest valid NavMesh position
                NavMeshHit hit;
                if (NavMesh.SamplePosition(targetPosition, out hit, 100f, NavMesh.AllAreas))
                {
                    navMeshAgent.SetDestination(hit.position);
                }
                else
                {
                    // Fallback: use a random point if sampling fails
                    Vector3 randomDirectionFallback = Random.insideUnitSphere * 20f;
                    randomDirectionFallback += transform.position;
                    NavMesh.SamplePosition(randomDirectionFallback, out hit, 100f, NavMesh.AllAreas);
                    navMeshAgent.SetDestination(hit.position);
                }
            }
            else
            {
                // Fallback behavior if player is null
                Vector3 randomDirection = Random.insideUnitSphere * 20f;
                randomDirection += transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, 100f, NavMesh.AllAreas);
                navMeshAgent.SetDestination(hit.position);
            }
        }

        // Check if player is found to transition to prowling/stalking
        if (playerDistance < CalculateDetectionRange())
        {
            ChooseHuntingMethod();
        }
    }

    private void HandleProwlingState()
    {
        navMeshAgent.isStopped = false;

        // Only set destination once when entering this state
        if (!hasSetProwlingDestination && player != null)
        {
            navMeshAgent.SetDestination(player.position);
            navMeshAgent.speed = CalculateProwlingSpeed();
            hasSetProwlingDestination = true;

            // Play distinct sound cue
            AudioManager.Instance.PlaySound("TigerProwl", gameObject);
        }

        // Check if tiger is close enough to the player to attack outright.
        if (playerDistance < 7.5f)
        {
            // Attack based on aggressiveness
            if (ShouldAttackBasedOnAggressiveness())
            {
               // animator.SetBool("isAttacking", true);
                AttackPlayer();
            }
        }

        if (playerDistance > 20)
        {
            prowlChase = true;
            TransitionToState(TigerState.Chase);
        }

        if (!isCheckingLook)
        {
            StartCoroutine(ProwlingPlayerLookingCheck());
        }

    }

    IEnumerator ProwlingPlayerLookingCheck()
    {
        isCheckingLook = true;
        if (IsPlayerLookingAtTiger())
        {
            yield return new WaitForSeconds(2);
            if (IsPlayerLookingAtTiger())
            {
                stalkChase = true;
                TransitionToState(TigerState.Chase);
                isCheckingLook = false;
            }
        }
        isCheckingLook = false;
    }

    private void HandleStalkingState()
    {
        navMeshAgent.isStopped = false;

        // Only set destination once when entering this state
        if (!hasSetStalkingDestination)
        {
            Vector3 stalkPosition = CalculateStalkPosition();
            navMeshAgent.SetDestination(stalkPosition);
            hasSetStalkingDestination = true;

            // Play distinct sound cue
            AudioManager.Instance.PlaySound("TigerStalking", gameObject);
        }


        // Stalking duration based on aggressiveness
        if (ShouldStopStalking() || playerDistance > 35)
        {
            TransitionToState(TigerState.Evacuating);
        }

        if (!isCheckingLook)
        {
            StartCoroutine(StalkingPlayerLookingCheck());
        }

        if (!isCheckingAware)
        {
            StartCoroutine(StalkingPlayerAwarenessCheck());
        }
    }

    IEnumerator StalkingPlayerLookingCheck()
    {
        isCheckingLook = true;
        if (!IsPlayerLookingAtTiger())
        {
            yield return new WaitForSeconds(2);
            if (!IsPlayerLookingAtTiger())
            {
                Debug.Log("KEEP LOOKING");
                stalkChase = true;
                TransitionToState(TigerState.Chase);
                isCheckingLook = false;
            }
        }
        isCheckingLook = false;
    }

    IEnumerator StalkingPlayerAwarenessCheck()
    {
        isCheckingAware = true;

        if (awareness > 1f)
        {
            yield return new WaitForSeconds(2);
            if (awareness > 1f)
            {
                Debug.Log("NO RUNNING");
                stalkChase = true;
                TransitionToState(TigerState.Chase);
            }
        }
        isCheckingAware = false;
    }

    private void HandleEvacuatingState()
    {
        navMeshAgent.isStopped = false;

        navMeshAgent.speed = 10f;

        // Only set destination once when entering this state
        if (!hasSetEvacuatingDestination)
        {
            Vector3 evacuatePosition = CalculateEvacuatePosition();
            navMeshAgent.SetDestination(evacuatePosition);
            hasSetEvacuatingDestination = true;

            // Play evacuation sound cue
            AudioManager.Instance.PlaySound("TigerEvacuate", gameObject);
        }

        // Check if reached destination
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
            {
                // Return to hunting/searching or idle
                TransitionToState(TigerState.HuntingSearching);
            }
        }
    }

    private void HandleChaseState()
    {
        navMeshAgent.SetDestination(player.position);

        if (stalkChase)
        {
            navMeshAgent.speed = 15f;
        }
        if (prowlChase)
        {
            navMeshAgent.speed = 6f;
        }

        if (playerDistance < 3f)
        {
            // Attack based on aggressiveness
            if (ShouldAttackBasedOnAggressiveness())
            {
               // animator.SetBool("isAttacking", true);
                AttackPlayer();
            }
        }

        if (playerDistance > 30)
        {
            stalkChase = false;
            prowlChase = false;
            TransitionToState(TigerState.Evacuating);
        }
    }
    #endregion

    #region Helper Methods
    public void TransitionToState(TigerState newState)
    {
        // Exit current state logic
        switch (currentState)
        {
            case TigerState.Idle:
                // Clean up idle state
                break;
        }

        // Enter new state logic
        currentState = newState;
        stateTransitionTimer = 0f;

        switch (newState)
        {
            case TigerState.Idle:
                // Initialize idle state
                break;
        }
    }

    private float CalculateHuntingSpeed()
    {
        // Base speed modified by aggressiveness
        return 3f + (fullAgressiveness * 0.5f);
    }

    private float CalculateProwlingSpeed()
    {
        // Slower speed for prowling
        return 2f + (fullAgressiveness * 0.2f);
    }

    private float CalculateDetectionRange()
    {
        // Detection range based on aggressiveness
        return 15f + (fullAgressiveness * 0.5f);
    }

    private void ChooseHuntingMethod()
    {
        // Random choice between prowling and stalking
        if (Random.value > 0.5f)
        {
            TransitionToState(TigerState.Prowling);
        }
        else
        {
            TransitionToState(TigerState.Stalking);
        }
    }

    private bool IsPlayerLookingAtTiger()
    {
        // Get the player's camera
        Camera playerCamera = Camera.main;
        if (playerCamera == null) return false;

        // Calculate the direction from camera to tiger
        Vector3 directionToTiger = (transform.position - playerCamera.transform.position).normalized;

        // Check if tiger is within the camera's field of view
        float angleToTiger = Vector3.Angle(playerCamera.transform.forward, directionToTiger);

        if (angleToTiger <= playerCamera.fieldOfView * 0.6f) // 60% of FOV for center focus
        {
            // Raycast to check if there's line of sight
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, directionToTiger, out hit, Mathf.Infinity))
            {
                // Check if the ray hit the tiger or its collider
                if (hit.collider.transform == transform || hit.collider.transform.IsChildOf(transform))
                {
                    //Debug.Log("TIGER SPOTTED");
                    return true;
                }
            }
        }

        return false;
    }

    private bool ShouldAttackBasedOnAggressiveness()
    {
        // Higher aggressiveness = faster attack decision
        float attackChance = fullAgressiveness / maxAggressiveness + 3;
        return Random.value < attackChance * Time.deltaTime;
    }

    private bool ShouldStopStalking()
    {
        // Stalking duration based on aggressiveness
        float maxStalkTime = 30f - (fullAgressiveness * 1f);
        return stateTransitionTimer >= maxStalkTime;
    }

    private Vector3 CalculateStalkPosition()
    {
        // Calculate position at a distance from player
        Vector3 directionFromPlayer = (transform.position - player.position).normalized;
        return player.position + directionFromPlayer * (10f + fullAgressiveness * 0.5f);
    }

    private Vector3 CalculateEvacuatePosition()
    {
        if (player == null)
        {
            // Fallback if player is missing
            Vector3 randomDirection = Random.insideUnitSphere * 200f;
            NavMeshHit hit;
            NavMesh.SamplePosition(transform.position + randomDirection, out hit, 200f, NavMesh.AllAreas);
            return hit.position;
        }

        // Move to outer ring around player (at least 125 units away)
        int maxAttempts = 10; // Prevent infinite loop
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            // Get a random direction on the XZ plane (ignore Y for horizontal movement)
            Vector2 randomCircle = Random.insideUnitCircle.normalized;
            Vector3 randomDirection = new Vector3(randomCircle.x, 0f, randomCircle.y);

            // Scale to minimum distance + some randomness
            float distance = 125f + Random.Range(0f, 50f);
            Vector3 targetPosition = player.position + randomDirection * distance;

            // Find the nearest valid NavMesh position
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPosition, out hit, 100f, NavMesh.AllAreas))
            {
                // Verify it's actually far enough from the player
                float distanceToPlayer = Vector3.Distance(hit.position, player.position);
                if (distanceToPlayer >= 150f)
                {
                    return hit.position;
                }
            }

            attempts++;
        }

        // Fallback if all attempts fail - just move away from player
        Vector3 awayFromPlayer = (transform.position - player.position).normalized;
        Vector3 fallbackPosition = player.position + awayFromPlayer * 200f;
        NavMeshHit fallbackHit;
        NavMesh.SamplePosition(fallbackPosition, out fallbackHit, 200f, NavMesh.AllAreas);
        return fallbackHit.position;
    }

    private void TeleportToRandomLocationNearPlayer()
    {
        if (player == null) return;

        int maxAttempts = 10;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            // Get random direction on XZ plane
            Vector2 randomCircle = Random.insideUnitCircle.normalized;
            Vector3 randomDirection = new Vector3(randomCircle.x, 0f, randomCircle.y);

            // Random distance
            float distance = Random.Range(teleportMinDistance, teleportMaxDistance);
            Vector3 targetPosition = player.position + randomDirection * distance;

            // Sample only walkable positions
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPosition, out hit, 100f, NavMesh.AllAreas))
            {
                navMeshAgent.Warp(hit.position);
                Debug.Log("Tiger teleported to (walkable): " + hit.position);
                return;
            }

            attempts++;
        }

        Debug.LogWarning("Failed to find valid walkable teleport location after " + maxAttempts + " attempts");
    }

    private void AttackPlayer()
    {
        isAttacking = true;

        animator.SetBool("isAttacking", true);

        //Putting this here for your use when you implement - Conner
        AudioManager.Instance.PlaySound("TigerAttack", gameObject);

        // Implement attack logic
        GameManager.instance.OnDeath.Invoke();

        // DEMO VERSION
        //gameObject.SetActive(false);
    }
    #endregion

    // Public method to be called when bodies are found
    public void OnBodyFound()
    {
        aggressiveness += 1f;
        aggressiveness = Mathf.Clamp(aggressiveness, 0, maxAggressiveness);
    }

    
}