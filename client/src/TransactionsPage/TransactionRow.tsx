import React, { useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { CategoriesResponse, TransactionResponse } from "../Contracts/Responses";
import Amount from "../Amount";
import Timestamp from "../Timestamp";

interface TransactionRowProps {
    transaction: TransactionResponse,
    isCategorizing: boolean,
    onCategorized: (id: number, moveNext: boolean) => void
}

export default function TransactionRow({ transaction, isCategorizing, onCategorized }: TransactionRowProps) {
    const [categorizing, setCategorizing] = useState(false);
    const [category, setCategory] = useState<string | null>(transaction.category ?? null);

    const queryClient = useQueryClient();

    const { data } = useQuery<CategoriesResponse>({
        queryKey: ['categories'],
        queryFn: async () => await fetch('https://localhost:5001/categories')
            .then((res) => res.json())
    });

    const updateMutation = useMutation({
        mutationFn: async (category: string | null) => {
            await fetch(`https://localhost:5001/transactions/${transaction.id}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ ...transaction, category: category })
            });
        },
        onSuccess: () => queryClient.invalidateQueries(['transactions'])
    });

    async function categorize(category: string | null, moveNext: boolean) {
        transaction.category = category ?? undefined;
        await updateMutation.mutateAsync(category);
        queryClient.invalidateQueries(['categories']);
        setCategorizing(false);
        onCategorized(transaction.id, moveNext);
    }

    async function finishInput(moveNext: boolean) {
        if (!!category?.trim()) {
            await categorize(category.trim(), moveNext);
        }
        else {
            await categorize(null, moveNext);
        }
    }

    async function blur() {
        await finishInput(false);
    }

    function input(e: React.ChangeEvent<HTMLInputElement>) {
        setCategory(e.target.value);
    }

    async function keyDown(e: React.KeyboardEvent<HTMLInputElement>) {
        if (e.key === "Enter") {
            await finishInput(true);
        }
    }

    return (
        <tr className="odd:bg-gray-100">
            <td className="px-1 pl-2"><Timestamp ts={transaction.timestamp}></Timestamp></td>
            <td className="px-1 text-right"><Amount amount={transaction.amount}/></td>
            <td className="px-1">{transaction.description}</td>
            <td className="px-1">{transaction.account}</td>
            <td className="px-1">
                {!(categorizing || isCategorizing) ? <button className={`w-full text-left ${transaction.category ? "" : "text-gray-400"}`} onClick={_ => setCategorizing(true)}>{transaction.category ?? "Add category"}</button>
                    : (
                        <>
                            <input className="bg-[inherit] focus:bg-gray-200 w-full h-full" list="categoryList"
                                value={category ?? undefined}
                                autoFocus
                                onKeyDown={keyDown}
                                onInput={input}
                                onBlur={blur}/>
                            {!!data &&
                            <datalist id="categoryList">
                                {data.categories.map(c => <option value={c}></option>)}
                            </datalist>
                            }
                        </>
                    )}
            </td>
        </tr>
    );
}
