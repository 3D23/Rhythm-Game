using System;

[Serializable]
public class PlayerData
{
    public enum PlayerDataFields
    {
        Money
    }

    public int Money { get; private set; }

    public void Set(PlayerDataFields key, object value)
    {
        switch (key)
        {
            case PlayerDataFields.Money:
                Money = (int)value; 
                break;
        }
    }

    public object Get(PlayerDataFields key)
    {
        return key switch
        {
            PlayerDataFields.Money => Money,
            _ => null,
        };
    }
}
