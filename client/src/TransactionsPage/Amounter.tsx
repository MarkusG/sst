import {
  CategorizationResponse,
  TransactionResponse,
} from "../Contracts/Responses.ts";
import React, { useContext, useState } from "react";
import { Focus, TransactionTableContext } from "./TransactionTableContext.tsx";
import { AfterAmountChangedAction } from "./AfterAmountChangedAction.ts";
import Amount from "../Amount.tsx";

export interface AmounterProps {
  transaction: TransactionResponse;
  categorization?: CategorizationResponse;
  onUpdated: (amount: number, after: AfterAmountChangedAction) => any;
}

export default function Amounter({
  transaction,
  categorization,
  onUpdated,
}: AmounterProps) {
  const [rawAmount, setRawAmount] = useState<string>("0");
  const [amount, setAmount] = useState<number>(0);
  const [context, setContext] = useContext(TransactionTableContext);

  async function finishInput(after: AfterAmountChangedAction) {
    const parsedAmount = Number(rawAmount);

    if (!isNaN(parsedAmount)) {
      setAmount(parsedAmount);
      setRawAmount(parsedAmount.toString());
    }

    await onUpdated(parsedAmount, after);
  }

  async function blur() {
    await finishInput(AfterAmountChangedAction.Deselect);
  }

  function input(e: React.ChangeEvent<HTMLInputElement>) {
    setRawAmount(e.target.value);
  }

  async function keyDown(e: React.KeyboardEvent<HTMLInputElement>) {
    if (e.key === "Enter") {
      await finishInput(AfterAmountChangedAction.MoveNext);
    }
  }

  function select() {
    setContext({
      transactionId: transaction.id,
      categorizationId: categorization?.id,
      focus: Focus.Category,
    });
  }

  if (
    context.transactionId !== transaction.id ||
    context.categorizationId !== categorization?.id ||
    context.focus !== Focus.Amount
  ) {
    return (
      <button className="w-full text-right" onFocus={select}>
        <Amount amount={amount} />
      </button>
    );
  }

  return (
    <input
      className="bg-[inherit] focus:bg-gray-200 w-full h-full text-right"
      value={rawAmount}
      autoFocus
      onKeyDown={keyDown}
      onInput={input}
      onBlur={blur}
    />
  );
}
