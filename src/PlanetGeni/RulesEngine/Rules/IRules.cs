using System;
using System.ComponentModel.DataAnnotations;

namespace RulesEngine
{
    public interface IRules
    {
        ValidationResult IsValid();
    }
}
