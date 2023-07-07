using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using DG.Tweening;
using YH;



public class Test : MonoSingleton<Test>//快速单例：直接继承即可
{
    private void Start()
    {
        Sequence myDotweenSequence = DOTween.Sequence();
        
        //dot ween测试
        myDotweenSequence.Append(
        transform.DOMove(Vector3.one, 2));
        myDotweenSequence.Append(
            transform.DORotate(Vector3.up*180,2));
        myDotweenSequence.OnComplete(() =>
        {
            print("结束动画");
            
            
            //彩色调试信息API
            YH.Debug.Logger.PrintHint("指示信息测试");
            YH.Debug.Logger.PrintWarning("警告信息测试");
            YH.Debug.Logger.PrintError("错误信息测试");

        });
        
        
        //对象池使用示例--------------------------------------------------
        ObjectPool<Dog> dogPool=new ObjectPool<Dog>(()=> new Dog());//池的构造函数中需要提供一个构造该池中对象的方法
        dogPool.SetPoolName("dogPool");//名称有助于输出调试信息，但不填也无所谓
        dogPool.AutoFillPoolIncrement = 3;//这个是池中对象不够提取时自动申请对象的数量,不填也无所谓
        
        List<Dog> buffer=new List<Dog>();
        //提取对象
        for (int i = 0; i < 10; i++)
        {
            buffer.Add(dogPool.Depool());
        }
        
        //归还对象
        foreach (var obj in buffer)
        {
            dogPool.Enpool(obj);
        }
        buffer.Clear();
        
        
        
        
        //对象池管理器示例--------------------------------------------------
        //共用对象的对象池还可放入池管理器中以供全局访问
        PoolManager.AddNewPool(dogPool);
        
        //下次访问时可直接通过泛型拿到相应的池中对象:
        var dog=PoolManager.Depool<Dog>();
        //或者直接自己拿到pool后提取对象
        //PoolManager.GetPool<Dog>().Depool();
        
        dog.Bite();
        
        //归还对象
        PoolManager.Enpool(dog);

    }

    private void Update()
    {
    }
}

public class Dog
{
    private int age;

    public void Bite()
    {
        Debug.Log("AAAAAAAAAA");
    }

    public Dog()
    {
        Bite();
    }
}
