import { QueryFunctionContext, useQuery } from "@tanstack/react-query";
import { CashFlowResponse } from "../Contracts/Responses";
import LoadingIcon from "../LoadingIcon/LoadingIcon";
import Amount from "../Amount";
import { useState } from "react";

async function query({ queryKey }: QueryFunctionContext) : Promise<CashFlowResponse> {
    const params = queryKey[1] as number;
    return await fetch(`https://localhost:5001/cashflow?year=${params}`)
        .then((res) => res.json())
}

function CashflowPage() {
    const [enableYearButtons, setEnableYearButtons] = useState(true);
    const [year, setYear] = useState(new Date().getFullYear());
    const [rawYear, setRawYear] = useState(year.toString());
    const [yearInputEnabled, setYearInputEnabled] = useState(false);

    const { data, error, isError, isLoading } = useQuery<CashFlowResponse>({
        queryKey: ['cashflow', year],
        queryFn: query
    });

    function onYearInputBlur() {
        const value = Number(rawYear);

        if (isNaN(value) || value === 0) {
            setRawYear(new Date().getFullYear().toString());
            return;
        }
        else if (value > new Date().getFullYear()) {
            setYear(new Date().getFullYear());
            setRawYear(year.toString());
        } else
            setYear(value);

        setEnableYearButtons(false);
        setYearInputEnabled(false);
    }

    function onYearInput(e: React.ChangeEvent<HTMLInputElement>) {
        const value = Number(e.target.value);
        if (isNaN(value))
            return;

        setRawYear(value.toString());

        if (value === 0)
            setRawYear('');
    }

    function onYearInputKeyDown(e: React.KeyboardEvent<HTMLInputElement>) {
        if (e.key === "Enter") {
            onYearInputBlur();
        }
    }

    function nextYear() {
        if (!enableYearButtons) {
            setEnableYearButtons(true);
            return;
        }

        if (year === new Date().getFullYear())
            return;

        setYear(year + 1);
        setRawYear((year + 1).toString());
    }

    function previousYear() {
        if (!enableYearButtons) {
            setEnableYearButtons(true);
            return;
        }

        if (year === 1)
            return;

        setYear(year - 1);
        setRawYear((year - 1).toString());
    }

    return (
        <div className="pt-2 h-full overflow-auto">
            <h1 className="text-3xl text-center mb-1">
            <span className="mr-2 inline-grid grid-rows-1 grid-cols-[min-content,_1fr,_min-content] gap-1 items-baseline">
                <button className="text-base text-gray-500 disabled:text-gray-300 h-full self-center"
                    disabled={yearInputEnabled}
                    onClick={previousYear}>
                    <i className="fa fa-chevron-left"></i>
                </button>
                {!yearInputEnabled &&
                    <button className="w-[4.7rem] bg-gray-150 hover:bg-gray-300 transition px-1" onClick={() => setYearInputEnabled(true)}>{year}</button>
                }
                {yearInputEnabled &&
                    <input className="w-[4.7rem] bg-gray-100 px-1"
                        autoFocus
                        onKeyDown={onYearInputKeyDown}
                        onInput={onYearInput}
                        onFocus={e => e.target.select()}
                        onBlur={onYearInputBlur}
                        value={rawYear}/>
                }
                <button className="text-base text-gray-500 disabled:text-gray-300 h-full self-center"
                    disabled={yearInputEnabled}
                    onClick={nextYear}>
                    <i className="fa fa-chevron-right"></i>
                </button>
            </span>
                Cash Flow
            </h1>
            {isLoading && <div className="w-min m-auto mt-4"><LoadingIcon/></div>}
            {isError && 
                <div className="p-2">
                    <p className="text-xl">{error!.toString()}</p>
                </div>
            }
            {!(isLoading || isError) &&
                <div className="grid grid-rows-auto grid-cols-[2fr,_repeat(13,_1fr)] text-right">
                        <div className="pl-2 text-left border-b border-gray-300">Category</div>
                        <div className="border-b border-gray-300 px-2">Jan</div>
                        <div className="border-b border-gray-300 px-2">Feb</div>
                        <div className="border-b border-gray-300 px-2">Mar</div>
                        <div className="border-b border-gray-300 px-2">Apr</div>
                        <div className="border-b border-gray-300 px-2">May</div>
                        <div className="border-b border-gray-300 px-2">Jun</div>
                        <div className="border-b border-gray-300 px-2">Jul</div>
                        <div className="border-b border-gray-300 px-2">Aug</div>
                        <div className="border-b border-gray-300 px-2">Sep</div>
                        <div className="border-b border-gray-300 px-2">Oct</div>
                        <div className="border-b border-gray-300 px-2">Nov</div>
                        <div className="border-b border-gray-300 px-2">Dec</div>
                        <div className="border-b border-gray-300 px-2">Total</div>
                        {data.categories.map(c => <>
                            <div className="contents [&_div]:even:bg-gray-100">
                                <div className="text-left pl-2 bg-inherit">{c.category}</div>
                                <div className="px-2"><Amount amount={c.january}/></div>
                                <div className="px-2"><Amount amount={c.february}/></div>
                                <div className="px-2"><Amount amount={c.march}/></div>
                                <div className="px-2"><Amount amount={c.april}/></div>
                                <div className="px-2"><Amount amount={c.may}/></div>
                                <div className="px-2"><Amount amount={c.june}/></div>
                                <div className="px-2"><Amount amount={c.july}/></div>
                                <div className="px-2"><Amount amount={c.august}/></div>
                                <div className="px-2"><Amount amount={c.september}/></div>
                                <div className="px-2"><Amount amount={c.october}/></div>
                                <div className="px-2"><Amount amount={c.november}/></div>
                                <div className="px-2"><Amount amount={c.december}/></div>
                                <div className="px-2 border-l border-gray-300"><Amount amount={c.total}/></div>
                            </div>
                        </>)}
                        <div className="contents [&_div]:even:bg-gray-100">
                            <div className="text-left pl-2 bg-inherit border-t border-gray-300">Total</div>
                            <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.january}/></div>
                            <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.february}/></div>
                            <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.march}/></div>
                            <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.april}/></div>
                            <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.may}/></div>
                            <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.june}/></div>
                            <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.july}/></div>
                            <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.august}/></div>
                            <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.september}/></div>
                            <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.october}/></div>
                            <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.november}/></div>
                            <div className="px-2 border-t border-gray-300"><Amount amount={data.totals.december}/></div>
                            <div className="px-2 border-t border-l border-gray-300"><Amount amount={data.totals.total}/></div>
                        </div>
                </div>
            }
        </div>
    )
}

export default CashflowPage;
