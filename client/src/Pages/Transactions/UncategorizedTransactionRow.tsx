import Timestamp from "../../Timestamp.tsx";
import Amount from "../../Amount.tsx";
import Categorizer from "./Categorizer.tsx";
import TransactionRowProps from "./TransactionRowProps.ts";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AfterCategorizationAction } from "./AfterCategorizationAction.ts";

export default function UncategorizedTransactionRow({
  transaction,
  onCategoryUpdated,
}: TransactionRowProps) {
  const queryClient = useQueryClient();

  const { mutateAsync: categorize } = useMutation({
    mutationFn: async (category: string | null) => {
      await fetch(
        `https://localhost:5001/transactions/${transaction.id}/categorizations/${category}`,
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ amount: transaction.amount, position: 0 }),
        },
      );
    },
    onSuccess: () => queryClient.invalidateQueries(["transactions"]),
  });

  async function categorized(
    category: string | null,
    after: AfterCategorizationAction,
  ) {
    if (category) await categorize(category);

    onCategoryUpdated(after);
  }

  return (
    <tr className="odd:bg-gray-100">
      <td className="px-1 pl-2">
        <Timestamp ts={transaction.timestamp} />
      </td>
      <td className="px-1">{transaction.account}</td>
      <td className="px-1 overflow-hidden text-ellipsis">
        {transaction.description}
      </td>
      <td className="px-1 text-right">
        <Amount amount={transaction.amount} />
      </td>
      <td className="px-1">
        <Categorizer transaction={transaction} onCategorized={categorized} />
      </td>
    </tr>
  );
}
