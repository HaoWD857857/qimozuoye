using Unity.VisualScripting;
//���ܽӿ�
public interface ISkill 
{
    void Activate(); //�����
    void ResetCooldown();//������ȴʱ��
    bool IsOnCooldown();//�ж��Ƿ�����ȴ
}
