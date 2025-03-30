using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace SeraphUtil
{
    /// <summary>
    /// A wrapper class for a stat, allowing the subscription of additive and multiplicative methods to the stat. Performing additive transformations before multiplicative.
    /// </summary>
    public abstract class Stat<TValue, TEntity> 
    {
        /// <summary>
        /// A delegate to a method that takes a ref of the current value, and transforms it additively (+/-) or multiplicatively (*/%).
        /// </summary>
        public delegate void TransformerDelegate(ref TValue value, TEntity entity);

        /// <summary>
        /// The base value of the stat before transformations.
        /// </summary>
        public TValue Base { get; set; }
        
        /// <summary>
        /// Subscribe methods that will transform the stat additively (+/-)
        /// </summary>
        public TransformerDelegate Additive { get; set; }
        /// <summary>
        /// Subscribe methods that will transform the stat multiplicatively (+/-)
        /// </summary>
        public TransformerDelegate Multiplicative { get; set; }

        
        protected Stat(TValue baseValue)
        {
            Base = baseValue;
        }

        /// <summary>
        /// Calculates the stat after adding a certain value, and then doing all the transformations.
        /// </summary>
        /// <param name="entity"> the entity that uses the stat </param>
        /// <param name="addedValue"> the amount to add to the base value.</param>
        /// <returns></returns>
        public TValue FinalWithAdditive(TEntity entity, TValue addedValue)
        {
            return CalculateFinal(entity, addedValue);
        }

        /// <summary>
        /// The final value of the stat after all the transformations.
        /// </summary>
        public TValue Final(TEntity entity)
        {
            return CalculateFinal(entity);
        }
        private TValue CalculateFinal(TEntity entity, TValue addedValue = default(TValue))
        {
            var final = Add(Base, addedValue);
            Additive?.Invoke(ref final, entity);
            Multiplicative?.Invoke(ref final, entity);
            return final;
        }

        protected abstract TValue Add(TValue x, TValue y);

    }
    
    /// <summary>
    /// A wrapper class for an integer stat, allowing the subscription of additive and multiplicative methods to the stat. Performing additive transformations before multiplicative.
    /// </summary>
    public class StatInt<T> : Stat<int, T> 
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
    public class StatFloat<T> : Stat<float,T> 
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

