using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct BallSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BallData>();
    }
    
    public partial struct BallJob : IJobEntity
    {
        public float deltaTime;
        [BurstCompile(CompileSynchronously = true)]
        public void Execute(ref BallData ball, ref LocalTransform transform)
        {
            ball.time += deltaTime;
            float currentSpeed = math.sin(ball.time) * ball.speed;
            transform = transform.Translate(ball.direction * currentSpeed * deltaTime);
        }
    }

    public void OnUpdate(ref SystemState state)
    {
        BallJob job = new BallJob()
        {
            deltaTime = SystemAPI.Time.DeltaTime
        };
        job.ScheduleParallel();
    }
}
