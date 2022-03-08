#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> IBootstrapperService.cs </Name>
//         <Created> 19/04/2018 6:45:35 PM </Created>
//         <Key> c0a24db6-6d49-4540-b7bd-aeb2eab7a9ae </Key>
//     </File>
//     <Summary>
//         IBootstrapperService.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using System.Threading;
using System.Threading.Tasks;

namespace TIGE.Contract.Service
{
    public interface IBootstrapperService
    {
        /// <summary>
        ///     Initial Database, Background Services and so on. 
        /// </summary>
        Task InitialAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Rebuild caching, configuration. 
        /// </summary>
        Task RebuildAsync(CancellationToken cancellationToken = default);
    }
}