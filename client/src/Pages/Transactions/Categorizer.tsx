import {
  CategoriesResponse,
  CategorizationResponse,
  TransactionResponse,
} from "../../Contracts/Responses.ts";
import React, { useContext, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { AfterCategorizationAction } from "./AfterCategorizationAction.ts";
import { Focus, TransactionTableContext } from "./TransactionTableContext.tsx";

export interface CategorizerProps {
  transaction: TransactionResponse;
  categorization?: CategorizationResponse;
  onCategorized: (
    category: string | null,
    after: AfterCategorizationAction,
  ) => any | Promise<any>;
}

export default function Categorizer({
  transaction,
  categorization,
  onCategorized,
}: CategorizerProps) {
  const [category, setCategory] = useState<string | undefined>(
    categorization?.category,
  );
  const [context, setContext] = useContext(TransactionTableContext);

  const { data } = useQuery<CategoriesResponse>({
    queryKey: ["categories"],
    queryFn: async () =>
      await fetch("https://localhost:5001/categories").then((res) =>
        res.json(),
      ),
  });

  async function finishInput(after: AfterCategorizationAction) {
    if (!!category?.trim()) {
      await onCategorized(category.trim(), after);
    } else {
      await onCategorized(null, after);
    }
  }

  async function blur() {
    await finishInput(AfterCategorizationAction.Deselect);
  }

  function input(e: React.ChangeEvent<HTMLInputElement>) {
    setCategory(e.target.value);
  }

  async function keyDown(e: React.KeyboardEvent<HTMLInputElement>) {
    if (e.key === "Enter") {
      const action = e.ctrlKey
        ? AfterCategorizationAction.CreateNew
        : AfterCategorizationAction.MoveNext;
      await finishInput(action);
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
    context.focus !== Focus.Category
  ) {
    return (
      <button
        className={`w-full text-left ${categorization ? "" : "text-gray-400"}`}
        onFocus={select}
      >
        {categorization?.category ?? "Add category"}
      </button>
    );
  }

  return (
    <>
      <input
        className="bg-[inherit] focus:bg-gray-200 w-full h-full"
        list="categoryList"
        value={category ?? ""}
        autoFocus
        onKeyDown={keyDown}
        onInput={input}
        onBlur={blur}
      />
      {!!data && (
        <datalist id="categoryList">
          {data.categories.map((c) => (
            <option value={c} key={c}></option>
          ))}
        </datalist>
      )}
    </>
  );
}
