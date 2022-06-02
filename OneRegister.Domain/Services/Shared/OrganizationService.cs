using Microsoft.EntityFrameworkCore;
using OneRegister.Data.Contract;
using OneRegister.Data.SuperEntities;
using OneRegister.Domain.Model.General;
using System;
using System.Collections.Generic;
using System.Linq;
using static OneRegister.Data.Contract.Constants;

namespace OneRegister.Domain.Services.Shared
{
    public class OrganizationService
    {
        public OrganizationService(IOrganizationRepository<Organization> organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        private readonly IOrganizationRepository<Organization> _organizationRepository;

        public void PathingOrganizations()
        {
            var roots = UpdateRootPaths();
            foreach (var root in roots)
            {
                UpdateChildPaths(root);
            }
        }

        private void UpdateChildPaths(Organization root)
        {
            var childs = _organizationRepository.Entities
                .Where(o => o.ParentId == root.Id)
                .OrderBy(o => o.Name)
                .ToList();
            if (childs.Any())
            {
                int sequencer = childs.Max(o => o.Sequencer);
                foreach (var child in childs)
                {
                    child.Sequencer = ++sequencer;
                    child.Path = root.Path + "." + sequencer.ToString();
                }
                _organizationRepository.UpdateAsAdmin(childs);
                foreach (var child in childs)
                {
                    UpdateChildPaths(child);
                }
            }
        }

        private List<Organization> UpdateRootPaths()
        {
            var roots = _organizationRepository.Entities
                .Where(o => o.ParentId == null && string.IsNullOrEmpty(o.Path))
                .OrderBy(o => o.Name)
                .ToList();
            if (roots.Any())
            {
                int rootSequncer = roots.Max(o => o.Sequencer);

                foreach (var root in roots)
                {
                    root.Sequencer = ++rootSequncer;
                    root.Path = rootSequncer.ToString();
                }
                _organizationRepository.UpdateAsAdmin(roots);
            }
            return roots;
        }

        public bool Any(Guid id)
        {
            return _organizationRepository.AnyByIdAsAdmin(id);
        }

        public Guid Add(Organization organization)
        {
            var parent = organization.ParentId.HasValue ? _organizationRepository.GetById(organization.ParentId.Value) : null;
            var lastChild = FindLastSibling(organization.ParentId);
            if (lastChild == null)
            {
                organization.Sequencer++;
                organization.Path = string.IsNullOrEmpty(parent?.Path) ? organization.Sequencer.ToString() : parent.Path + "." + organization.Sequencer.ToString();
            }
            else
            {
                organization.Sequencer = lastChild.Sequencer++;
                organization.Path = string.IsNullOrEmpty(parent?.Path) ? organization.Sequencer.ToString() : parent.Path + "." + organization.Sequencer.ToString();
            }

            _organizationRepository.Add(organization);
            return organization.Id;
        }

        private Organization FindLastSibling(Guid? parentId)
        {
            return _organizationRepository.Entities
                .Where(o => o.ParentId == parentId)
                .MaxBy(o => o.Sequencer);
        }

        public Guid SeedingAdd(Organization organization)
        {
            organization.CreatedBy = organization.ModifiedBy = BasicUser.AdminId;
            if (organization.State == StateOfEntity.Init)
            {
                organization.State = StateOfEntity.InProgress;
            }
            _organizationRepository.Add(organization);
            return organization.Id;
        }

        public Organization Get(Guid id)
        {
            return _organizationRepository.GetById(id);
        }

        public Dictionary<string, string> GetBaseOrganizationsDictionary()
        {
            var baseOrgs = GetBaseOrgs();
            return baseOrgs.ToDictionary(o => o.Id.ToString(), o => o.Name);
        }

        public OrgNode GetOrganizationHierarchy(OrgNode rootNode)
        {
            rootNode.Descendants = GetChildOrganization(rootNode.Id).Select(n => new OrgNode
            {
                Id = n.Id,
                Level = rootNode.Level + 1,
                Name = n.Name
            }).ToList();
            foreach (var node in rootNode.Descendants)
            {
                GetOrganizationHierarchy(node);
            }
            return rootNode;
        }

        public List<GijgoTreeNode> GetOrgTree()
        {
            var roots = _organizationRepository.Entities.Where(o => o.ParentId == null)
                .Select(o => new GijgoTreeNode
                {
                    Id = o.Id,
                    Text = o.Name
                })
                .ToList();
            roots.ForEach(o => o.Children = GetChildren(o.Id,_organizationRepository.Entities.ToList()));
            return roots;
        }

        private static List<GijgoTreeNode> GetChildren(Guid id,List<Organization> organizations)
        {
            return organizations.Where(o => o.ParentId == id)
                .Select(o => new GijgoTreeNode
                {
                    Id = o.Id,
                    Text = o.Name,
                    Children = GetChildren(o.Id, organizations)
                })
                .ToList();
        }

        public Dictionary<string, string> ListDescendants(OrgNode rootNode)
        {
            var descendantList = new Dictionary<string, string>
            {
                {rootNode.Id.ToString(), string.Concat(InsertLevelDash(rootNode.Level), rootNode.Name)}
            };
            foreach (var node in rootNode.Descendants.OrderBy(d => d.Name))
            {
                AddNodeToList(node, descendantList);
            }
            return descendantList;
            void AddNodeToList(OrgNode rootNode, Dictionary<string, string> descendantList)
            {
                descendantList.Add(rootNode.Id.ToString(), string.Concat(InsertLevelDash(rootNode.Level), rootNode.Name));
                foreach (var node in rootNode.Descendants.OrderBy(d => d.Name))
                {
                    descendantList.Add(node.Id.ToString(), string.Concat(InsertLevelDash(node.Level), node.Name));
                }
            }
        }

        private string InsertLevelDash(int level)
        {
            string dashes = string.Empty;
            for (int i = 0; i < level * 2; i++)
            {
                dashes += "-";
            }
            return dashes;
        }
        public List<Organization> GetBaseOrgs()
        {
            return _organizationRepository.Entities.AsNoTracking()
                .Where(o =>
                    o.Id == BasicOrganizations.OneRegister ||
                    o.Id == BasicOrganizations.School ||
                    o.Id == BasicOrganizations.Agropreneur ||
                    o.Id == BasicOrganizations.Merchant
                ).ToList();
        }
        private List<Organization> GetChildOrganization(Guid userOrganizationId)
        {
            return _organizationRepository.Entities.Where(o => o.Parent.Id == userOrganizationId).ToList();
        }
    }
}