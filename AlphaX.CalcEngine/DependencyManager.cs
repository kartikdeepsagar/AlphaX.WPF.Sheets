using AlphaX.CalcEngine.Parsers;
using System;
using System.Collections.Generic;

namespace AlphaX.CalcEngine
{
    /// <summary>
    /// Implementation of <see cref="IDependencyManager"/> that manages cell dependencies 
    /// using an <see cref="IDataProvider"/> to store and retrieve metadata.
    /// </summary>
    internal class DependencyManager : IDependencyManager
    {
        private readonly IDataProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyManager"/> class.
        /// </summary>
        /// <param name="provider">The data provider used to store metadata.</param>
        public DependencyManager(IDataProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        /// <inheritdoc />
        public void SetCellDependency(CellRef dependentCell, CellRef targetCell)
        {
            if (dependentCell == null) throw new ArgumentNullException(nameof(dependentCell));
            if (targetCell == null) throw new ArgumentNullException(nameof(targetCell));

            if (string.IsNullOrEmpty(targetCell.SheetName))
            {
                targetCell = new CellRef(targetCell.Row, targetCell.Column, dependentCell.SheetName);
            }

            var dependentSet = GetDependencySet(targetCell.SheetName, targetCell.Row, targetCell.Column, true);
            dependentSet.Add(dependentCell);
        }

        /// <inheritdoc />
        public void SetRangeDependency(CellRef dependentCell, CellRangeRef targetRange)
        {
            if (dependentCell == null) throw new ArgumentNullException(nameof(dependentCell));
            if (targetRange == null) throw new ArgumentNullException(nameof(targetRange));

            for (int r = targetRange.TopRow; r <= targetRange.BottomRow; r++)
            {
                for (int c = targetRange.LeftColumn; c <= targetRange.RightColumn; c++)
                {
                    SetCellDependency(dependentCell, new CellRef(r, c, targetRange.SheetName));
                }
            }
        }

        /// <inheritdoc />
        public IList<CellRef> GetDependentCells(string sheetName, int row, int column)
        {
            var dependents = new List<CellRef>();
            var dependentsSetQueue = new Queue<ISet<CellRef>>();
            
            var dependentSet = GetDependencySet(sheetName, row, column, false);
            if (dependentSet != null)
            {
                dependentsSetQueue.Enqueue(dependentSet);
            }

            while (dependentsSetQueue.Count > 0)
            {
                var currentSet = dependentsSetQueue.Dequeue();
                foreach (var dependent in currentSet)
                {
                    dependents.Add(dependent);
                    var nestedDependentSet = GetDependencySet(dependent.SheetName, dependent.Row, dependent.Column, false);
                    if (nestedDependentSet != null)
                    {
                        dependentsSetQueue.Enqueue(nestedDependentSet);
                    }
                }
            }

            return dependents;
        }

        /// <summary>
        /// Retrieves the dependency set for a cell, optionally creating an empty set if it doesn't exist.
        /// </summary>
        private ISet<CellRef> GetDependencySet(string sheetName, int row, int column, bool createEmptySetIfNull)
        {
            if (_provider.GetMetaData(sheetName, row, column) is CalcCellMetaInfo metaInfo)
            {
                if (metaInfo.Dependents == null && createEmptySetIfNull)
                {
                    metaInfo.Dependents = new HashSet<CellRef>();
                }
                return metaInfo.Dependents;
            }

            if (createEmptySetIfNull)
            {
                metaInfo = new CalcCellMetaInfo
                {
                    Dependents = new HashSet<CellRef>()
                };
                _provider.SetMetaData(sheetName, row, column, metaInfo);
                return metaInfo.Dependents;
            }

            return null;
        }
    }
}
