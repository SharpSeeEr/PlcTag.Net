using System.Collections.Generic;

namespace PlcTag
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
