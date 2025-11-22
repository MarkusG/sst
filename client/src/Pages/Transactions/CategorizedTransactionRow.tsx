import Timestamp from "../../Timestamp.tsx";
import Amount from "../../Amount.tsx";
import Categorizer from "./Categorizer.tsx";
import TransactionRowProps from "./TransactionRowProps.ts";
import { CategorizationResponse } from "../../Contracts/Responses.ts";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AfterCategorizationAction } from "./AfterCategorizationAction.ts";

export interface CategorizedTransactionRowProps extends TransactionRowProps {
  categorization: CategorizationResponse;
  showDetails: boolean;
}

export default function CategorizedTransactionRow({
  transaction,
  categorization,
  showDetails,
  onCategoryUpdated,
}: CategorizedTransactionRowProps) {
  const queryClient = useQueryClient();

  const { mutateAsync: update } = useMutation({
    mutationFn: async (category: string) => {
      await fetch(
        `https://localhost:5001/categorizations/${categorization.id}`,
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ category }),
        },
      );
    },
    onSuccess: () => queryClient.invalidateQueries(["transactions"]),
  });

  const { mutateAsync: uncategorize } = useMutation({
    mutationFn: async () => {
      await fetch(
        `https://localhost:5001/categorizations/${categorization.id}`,
        {
          method: "DELETE",
        },
      );
    },
    onSuccess: () => queryClient.invalidateQueries(["transactions"]),
  });

  async function categorized(
    category: string | null,
    after: AfterCategorizationAction,
  ) {
    if (category && category !== categorization.category)
      await update(category);
    if (!category) await uncategorize();

    onCategoryUpdated(after);
  }

  return (
    <tr className="odd:bg-gray-100">
      <td className="px-1 pl-2">
        {showDetails && <Timestamp ts={transaction.timestamp} />}
      </td>
      <td className="px-1">{showDetails && transaction.account}</td>
      <td className="px-1 overflow-hidden text-ellipsis">
        {showDetails && transaction.description}
      </td>
      <td className="px-1 text-right">
        <Amount amount={categorization.amount} />
      </td>
      <td className="px-1">
        <Categorizer
          transaction={transaction}
          categorization={categorization}
          onCategorized={categorized}
        />
      </td>
    </tr>
  );
}
