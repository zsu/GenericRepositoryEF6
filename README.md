[![NuGet](https://img.shields.io/nuget/v/GenericRepository.EF6.Mvc.svg)](https://www.nuget.org/packages/GenericRepository.EF6.Mvc)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

# What is GenericRepository

GenericRepository is a data access library using repository pattern.

Some of the features of GenericRepository are:

  * Build-in paging feature
  * Implement Unit of Work pattern
  * Support different Entity key types

# NuGet
```xml
Install-Package GenericRepository.EF6
```
# Getting started with GenericRepository

  * Annotate key property in entity classes with [Key] attribute
  * Get the repository object and call functions:
  ```xml
            var _uowProvider=new UowProvider();
            using (var uow = _uowProvider.CreateUnitOfWork<AppContext>())
            {
                var repository = uow.GetRepository<Department>();

                foreach (var item in buildings)
                {
                    repository.Add(item);
                }

                await uow.SaveChangesAsync();
            }
  ```

# License
All source code is licensed under MIT license - http://www.opensource.org/licenses/mit-license.php
