#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> Service.cs </Name>
//         <Created> 20/04/2018 10:59:38 AM </Created>
//         <Key> 17fcd85b-c501-4b04-a681-8cdd1e12d49a </Key>
//     </File>
//     <Summary>
//         Service.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using TIGE.Contract.Repository.Interfaces;

namespace TIGE.Service.Base
{
    public abstract class Service
    {
        protected readonly IUnitOfWork UnitOfWork;

        protected Service(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}