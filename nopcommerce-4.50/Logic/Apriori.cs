using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.IARelatedProducts.Logic
{
	public class ItemSet
	{
		public List<string> Items { get; set; }
		public int Count { get; set; }
		public double Support { get; set; }

		public double Confidence { get; set; }

		public ItemSet(List<string> items)
		{
			Items = items;
			Count = 0;
			Support = 0;
		}

		public override string ToString()
		{
			return string.Join(", ", Items);
		}
	}


	public class Apriori
	{

		// Función para generar candidatos de tamaño k a partir de conjuntos de elementos frecuentes de tamaño k-1
		List<ItemSet> GenerateCandidates(List<ItemSet> frequentItemSets)
		{
			List<ItemSet> candidates = new List<ItemSet>();
			// Combinar los conjuntos de elementos frecuentes de tamaño k-1 para generar candidatos de tamaño k
			for (int i = 0; i < frequentItemSets.Count; i++)
			{
				for (int j = i + 1; j < frequentItemSets.Count; j++)
				{
					ItemSet itemSet1 = frequentItemSets[i];
					ItemSet itemSet2 = frequentItemSets[j];

					if (itemSet1.Items[0].ToString() != itemSet2.Items[0].ToString())
					{
						List<string> mergedItems = new List<string>(itemSet1.Items);
						mergedItems.Add(itemSet2.Items.Last());
						ItemSet candidate = new ItemSet(mergedItems);
						candidates.Add(candidate);
					}
				}
			}

			return candidates;
		}


		// Función para obtener los conjuntos de elementos frecuentes
		public List<List<ItemSet>> GetFrequentItemSets(List<List<string>> transactions, double minSupport, double confidence)
		{
			transactions = transactions.Where(r => r.Count > 1).ToList();

			// Obtener la lista de elementos distintos
			HashSet<string> distinctItems = transactions.SelectMany(r => r).Distinct().ToHashSet();
			

			// Inicializar el conjunto de elementos frecuentes
			List<ItemSet> frequentItemSets = new List<ItemSet>();
			foreach (string item in distinctItems)
			{
				ItemSet itemSet = new ItemSet(new List<string> { item });
				itemSet.Count = transactions.Count(transaction => transaction.Contains(item));
				itemSet.Support = (double)itemSet.Count / transactions.Count;
				if (itemSet.Support >= minSupport)
				{
					frequentItemSets.Add(itemSet);
				}
			}

			// Obtener los conjuntos de elementos frecuentes de tamaño 2 o superior
			List<List<ItemSet>> result = new List<List<ItemSet>>();
			result.Add(frequentItemSets);
			
			// Generar candidatos
			List<ItemSet> candidates = GenerateCandidates(frequentItemSets);
			foreach (List<string> transaction in transactions)
			{
				// Contar los candidatos en la transacción actual
				foreach (ItemSet candidate in candidates)
				{
					if (!candidate.Items.Any(r => !transaction.Contains(r)))
					{
						candidate.Count++;
					}
				}
			}

			candidates.ForEach(r => r.Support = (double)r.Count / (double)transactions.Count);
			candidates.ForEach(r => r.Confidence = (double)(double)transactions.Count(rr => rr.Contains(r.Items[0]) && rr.Contains(r.Items[1])) / (double)transactions.Count(rr => rr.Contains(r.Items[0])));

			// Filtrar los candidatos por soporte mínimo y añadir los frecuentes a la lista de resultados
			frequentItemSets = candidates.Where(candidate => candidate.Support >= minSupport && candidate.Confidence >= confidence).ToList();
			
			if (frequentItemSets.Count > 0)
			{
				result.Add(frequentItemSets);
			}

			return result;
		}
	}
}
