using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private HeroFrame _heroFramePrefab;
    [SerializeField] private Transform _heroFrameContainter;
    
    public void AddHeroFrame(HeroUnit hero)
    {
        var frame = Instantiate(_heroFramePrefab, _heroFrameContainter);
        frame.Init(hero);
    }
    
}
