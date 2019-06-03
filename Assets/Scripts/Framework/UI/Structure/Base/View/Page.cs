using Framework.UI.Structure.Base.Model;
using UnityEngine;

namespace Framework.UI.Structure.Base.View
{
    public abstract class Page<T> : Screen where T : PageModel
    {
        [SerializeField] private T _model;

        public T Model
        {
            get { return _model; }
        }
    }
}