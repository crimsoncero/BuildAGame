using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace SeraphUtil
{
    /// <summary>
    /// A wrapper class for a stat, allowing the subscription of additive and multiplicative methods to the stat. Performing additive transformations before multiplicative.
    /// </summary>
    public abstract class Stat<T> 
    {
        /// <summary>
        /// A delegate to a method that takes a ref of the current value, and transforms it additively (+/-) or multiplicatively (*/%).
        /// </summary>
        public delegate void TransformerDelegate(ref T value);

        /// <summary>
        /// The base value of the stat before transformations.
        /// </summary>
        public T Base { get; set; }
        
        /// <summary>
        /// Subscribe methods that will transform the stat additively (+/-)
        /// </summary>
        public TransformerDelegate Additive { get; set; }
        /// <summary>
        /// Subscribe methods that will transform the stat multiplicatively (+/-)
        /// </summary>
        public TransformerDelegate Multiplicative { get; set; }

        /// <summary>
        /// The final value of the stat after all the transformations.
        /// </summary>
        public T Final { get { return CalculateFinal(); } }
        
        protected Stat(T baseValue)
        {
            Base = baseValue;
        }

        /// <summary>
        /// Calculates the stat after adding a certain value, and then doing all the transformations.
        /// </summary>
        /// <param name="addedValue"> the amount to add to the base value.</param>
        /// <returns></returns>
        public T FinalWithAdditive(T addedValue)
        {
            return CalculateFinal(addedValue);
        }
        
        private T CalculateFinal(T addedValue = default(T))
        {
            var final = Add(Base, addedValue);
            Additive?.Invoke(ref final);
            Multiplicative?.Invoke(ref final);
            return final;
        }

        protected abstract T Add(T x, T y);

    }
    
    /// <summary>
    /// A wrapper class for an integer stat, allowing the subscription of additive and multiplicative methods to the stat. Performing additive transformations before multiplicative.
    /// </summary>
    public class StatInt : Stat<int> 
    {
        public StatInt(int baseValue) : base(baseValue)
        {
        }

        protected override int Add(int x, int y)
        {
            return x + y;
        }
    }
    
    /// <summary>
    /// A wrapper class for a float stat, allowing the subscription of additive and multiplicative methods to the stat. Performing additive transformations before multiplicative.
    /// </summary>
    public class StatFloat : Stat<float> 
    {
        public StatFloat(float baseValue) : base(baseValue)
        {
        }

        protected override float Add(float x, float y)
        {
            return x + y;
        }
    }
    
    
    
}

