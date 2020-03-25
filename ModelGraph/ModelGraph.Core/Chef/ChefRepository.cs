using System;
using System.Collections.Generic;

namespace ModelGraph.Core
{/*

 */
    public partial class Chef
    {
        internal IRepository Repository;

        private void SaveToRepository()
        {
            if (Repository != null)
            {
                Repository.Write(this);
                CongealChanges();
            }
        }
        private void SaveToRepository(IRepository repository)
        {
            Repository = repository;
            SaveToRepository();
        }
        private string GetLongRepositoryName() => (Repository == null) ? NullStorageFileName : Repository.FullName;
        private string GetRepositoryName() => (Repository == null) ? NullStorageFileName : Repository.Name;
        private string NullStorageFileName => $"{_localize(GetNameKey(Trait.NewModel))} #{_newChefNumber}";


        internal void PostReadValidation()
        {
            ValidateQueryXStore();
        }
    }
}
