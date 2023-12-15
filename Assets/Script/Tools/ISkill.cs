using Unity.VisualScripting;
//技能接口
public interface ISkill 
{
    void Activate(); //激活技能
    void ResetCooldown();//重置冷却时间
    bool IsOnCooldown();//判断是否在冷却
}
