import Categorizer from "./Categorizer.tsx";
import TransactionRowProps from "./TransactionRowProps.ts";
import { CategorizationResponse } from "../Contracts/Responses.ts";
import Amounter from "./Amounter.tsx";
import { useState } from "react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AfterAmountChangedAction } from "./AfterAmountChangedAction.ts";
import { AfterCategorizationAction } from "./AfterCategorizationAction.ts";

export interface CategorizedTransactionRowProps extends TransactionRowProps {
  categorization: CategorizationResponse;
  index: number;
}

export default function ExtraCategorizationRow({
  categorization,
  transaction,
  onCategoryUpdated,
  onAmountUpdated,
}: CategorizedTransactionRowProps) {
  const [amount, setAmount] = useState<number>(0);

  const queryClient = useQueryClient();

  const mutation = useMutation({
    mutationFn: async (body: any) => {
      await fetch(
        `https://localhost:5001/transactions/${transaction.id}/categorizations/${body.category}`,
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            amount: body.amount,
            position: categorization.position + 1,
          }),
        },
      );
    },
    onSuccess: () => {
      queryClient.invalidateQueries(["transactions"]);
    },
  });

  async function amountUpdated(
    newAmount: number,
    after: AfterAmountChangedAction,
  ) {
    setAmount(newAmount);
    await onAmountUpdated(after);
  }

  async function categoryUpdated(
    category: string | null,
    after: AfterCategorizationAction,
  ) {
    if (transaction.categorizations.some((cz) => cz.category === category))
      return;

    await mutation.mutateAsync({ category, amount });
    await onCategoryUpdated(after);
  }

  return (
    <tr className="odd:bg-gray-100">
      <td className="px-1 pl-2"></td>
      <td className="px-1"></td>
      <td className="px-1"></td>
      <td className="px-1 text-right">
        <Amounter transaction={transaction} onUpdated={amountUpdated} />
      </td>
      <td className="px-1">
        <Categorizer
          transaction={transaction}
          onCategorized={categoryUpdated}
        />
      </td>
    </tr>
  );
}
