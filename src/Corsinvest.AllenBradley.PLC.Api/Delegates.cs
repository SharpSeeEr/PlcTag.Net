using System.Collections.Generic;

namespace Corsinvest.AllenBradley.PLC.Api
{
    /// <summary>
    /// Delegate change event Tag.
    /// </summary>
    /// <param name="result"></param>
    public delegate void EventHandlerOperation(OperationResult result);

    /// <summary>
    /// Delegate change event TagGroup.
    /// </summary>
    /// <param name="results"></param>
    public delegate void EventHandlerOperations(IEnumerable<OperationResult> results);
}