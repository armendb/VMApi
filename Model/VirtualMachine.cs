using System;
using System.Collections.Generic;
using VMApi.Util;

namespace VMApi.Model
{
    public class VirtualMachine
    {
        public int? Id { get; set; }

        public bool? IsStartable { get; set; }

        public string Location { get; set; }

        public string Owner { get; set; }

        public string CreatedBy { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public string[] Tags { get; set; }

        public int? Cpu { get; set; }

        public long? Ram { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? ParentId { get; set; }

        public bool? IsPatchable { get; set; }

        public int? ParentVMCount { get; set; }

        // TODO: Tests IsPatchable für 3. Tag im Monat, Edge Cases 2. Tag 23:59 etc.
        //       Tests ParentVMCount Case ohne parent, mit 1 parent, Endlosschleife
        public static IEnumerable<VirtualMachine> PrepareVMsData(IEnumerable<VirtualMachine> vms)
        {
            foreach (var vm in vms)
            {
                vm.IsPatchable = DateTime.Now.Day == 3 && vm.Status == VMStatus.Running;
                if (vm.ParentId.HasValue) vm.ParentVMCount = 1 + CalcParentVMCount(vm.ParentId.Value, vms);
            }

            return vms;
        }

        // TODO: Tests mit verschiedenen maxCounts und currentCounts
        private static int CalcParentVMCount(int parentId, IEnumerable<VirtualMachine> vms, int? maxCount = 10, int? currentCount = 0)
        {
            if (currentCount <= maxCount)
            {
                foreach (var vm in vms)
                {
                    if (vm.Id == parentId && vm.ParentId.HasValue)
                    {
                        return 1 + CalcParentVMCount(vm.ParentId.Value, vms, maxCount, ++currentCount);
                    }
                }
            }

            return 0;
        }
    }
}