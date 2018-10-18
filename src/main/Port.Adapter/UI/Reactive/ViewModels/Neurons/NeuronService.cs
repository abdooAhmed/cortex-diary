﻿using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace works.ei8.Cortex.Diary.Port.Adapter.UI.Reactive.ViewModels.Neurons
{
    public class NeuronService : INeuronService
    {
        public void Reload(SourceCache<NeuronDto, int> cache, NeuronDto neuronDto)
        {
            IEnumerable<NeuronDto> children = null;
            if (!cache.Items.Any(i => i.ParentId == neuronDto.Id))
            {
                children = NeuronService.CreateChildren(neuronDto);
            }
            else
            {
                children = cache.Items.Where(i => i.ParentId == neuronDto.Id);
                cache.Remove(children);
                // TODO: set children to data from cortex graph
            }

            cache.AddOrUpdate(children);
        }

        public void AddPostsynaptic(SourceCache<NeuronDto, int> cache, NeuronDto neuronDto)
        {
            var postsyn = new NeuronDto(
                Guid.NewGuid().GetHashCode(),
                neuronDto.Id,
                Guid.NewGuid().ToString(),
                neuronDto.ParentNeuronId,
                "New Postsynaptic",
                ChildType.Postsynaptic
                );

            cache.AddOrUpdate(postsyn);
        }

        public void AddPresynaptic(SourceCache<NeuronDto, int> cache, NeuronDto neuronDto)
        {
            var presyn = new NeuronDto(
                Guid.NewGuid().GetHashCode(),
                neuronDto.Id,
                Guid.NewGuid().ToString(),
                neuronDto.ParentNeuronId,
                "New Presynaptic",
                ChildType.Presynaptic
                );

            cache.AddOrUpdate(presyn);
        }

        // DEL: create dummy data
        internal static IEnumerable<NeuronDto> CreateChildren(NeuronDto parentDto = null)
        {
            var random = new Random();

            return Enumerable.Range(1, random.Next(1, 10))
                .Select(i =>
                {
                return new NeuronDto(
                    Guid.NewGuid().GetHashCode(),
                    parentDto == null ? 0 : parentDto.Id,
                    Guid.NewGuid().ToString(),
                    parentDto == null ? string.Empty : parentDto.ParentNeuronId,
                    $"Neuron {i}",
                    i % 2 == 0 ? ChildType.Postsynaptic : ChildType.Presynaptic
                    );
                });
        }

        public void ChangeData(SourceCache<NeuronDto, int> cache, NeuronDto dto, string value)
        {
            // TODO: update data source
            // TODO: retrieve updated value and replace value in children
            var newValue = new NeuronDto(
                dto.Id,
                dto.ParentId,
                dto.NeuronId,
                dto.ParentNeuronId,
                value,
                dto.Type
                );

            cache.AddOrUpdate(newValue);
        }
    }
}
