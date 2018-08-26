using MediatR;

namespace Api.Models.Queries
{
    public class RepresentativeQuery: IRequest<RepresentativeViewModel>
    {
        public string Address { get; set; }
    }
}
