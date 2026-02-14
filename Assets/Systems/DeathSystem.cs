using Unity.Entities;

public partial class DeathSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (dead, entity) in
                 SystemAPI.Query<IsDead>().WithEntityAccess())
        {
            ecb.DestroyEntity(entity);
        }

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
