--~ Copyright Ⓒ 2015 Victor Baybekov

package.path = package.path..";"..".\\?.lua;"..".\\?.luac"
package.cpath = package.cpath..";"..'.\\clibs\\?.dll'

local util = require("qsutils")

local qscallbacks = {}

--- Мы сохраняем пропущенные значения только если скрипт работает, но соединение прервалось
-- Если скрипт останавливается, то мы удаляем накопленные пропущенные значения
-- QuikSharp должен работать пока работает Квик, он не рассчитан на остановку внутри Квика.
-- При этом клиент может подключаться и отключаться сколько угодно и всегда получит пропущенные
-- сообщения после переподключения (если хватит места на диске)
local function CleanUp()
    -- close log
    pcall(logfile:close(logfile))
    -- discard missed values if any
    if missed_values_file then
        pcall(missed_values_file:close(missed_values_file))
        missed_values_file = nil
        pcall(os.remove, missed_values_file_name)
        missed_values_file_name = nil
    end
end

--- Функция вызывается когда соединение с QuikSharp клиентом обрывается
function OnQuikSharpDisconnected()
    -- TODO any recovery or risk management logic here
end

--- Функция вызывается когда скрипт ловит ошибку в функциях обратного вызова
function OnError(message)
    msg.cmd = "lua_error"
    msg.data = "Lua error: " .. message
    sendCallback(msg)
end

--- Функция вызывается терминалом QUIK при получении изменений текущей позиции по счету.
--- *только для брокеров https://forum.quik.ru/messages/forum10/message4434/topic506/#message4434
function OnAccountBalance(acc_bal)
    if is_connected then
        local msg = {}
	    msg.t = timemsec()
	    msg.cmd = "OnAccountBalance"
	    msg.data = acc_bal
	    sendCallback(msg)
    end
end

--- Функция вызывается терминалом QUIK при изменении денежной позиции по счету.
--- *предположительно как и OnAccountBalance
function OnAccountPosition(acc_pos)
    if is_connected then
        local msg = {}
        msg.t = timemsec()
        msg.cmd = "OnAccountPosition"
        msg.data = acc_bal
        sendCallback(msg)
    end
end

--- Функция вызывается терминалом QUIK при при изменении текущих параметров.
function OnParam(class_code, sec_code)
    if is_connected then
        local msg = {}
        msg.t = timemsec()
        msg.cmd = "OnParam"
        dat = {}
        dat.class_code = class_code
        dat.sec_code = sec_code
        msg.data = dat
        sendCallback(msg)
    end
end

--- Функция вызывается терминалом QUIK при установлении связи с сервером QUIK.
function OnConnected()
    if is_connected then
        local msg = {}
        msg.t = timemsec()
        msg.cmd = "OnConnected"
        msg.data = ""
        sendCallback(msg)
    end
end

--- Функция вызывается терминалом QUIK при отключении от сервера QUIK.
function OnDisconnected()
    if is_connected then
        local msg = {}
        msg.t = timemsec()
        msg.cmd = "OnDisconnected"
        msg.data = ""
        sendCallback(msg)
    end
end

--- Функция вызывается терминалом QUIK при получении изменений лимита по бумагам.
function OnDepoLimit (depo_limit)
    if is_connected then
        local msg = {}
        msg.t = timemsec()
        msg.cmd = "OnDepoLimit"
        msg.data = depo_limit
        sendCallback(msg)
    end
end

--- Функция вызывается терминалом QUIK при удалении клиентского лимита по бумагам.
function OnDepoLimitDelete (depo_limit_del)
    if is_connected then
        local msg = {}
        msg.t = timemsec()
        msg.cmd = "OnDepoLimitDelete"
        msg.data = depo_limit_del
        sendCallback(msg)
    end
end

--- Функция вызывается терминалом QUIK при получении обезличенной сделки.
function OnAllTrade(alltrade)
    if is_connected then
        local msg = {}
        msg.t = timemsec()
        msg.cmd = "OnAllTrade"
        msg.data = alltrade
        sendCallback(msg)
    end
end

--- Функция вызывается перед закрытием терминала QUIK.
function OnClose()
    if is_connected then
        local msg = {}
        msg.cmd = "OnClose"
        msg.t = timemsec()
        msg.data = ""
        sendCallback(msg)
    end
    CleanUp()
end

--- Функция вызывается терминалом QUIK перед вызовом функции main().
-- В качестве параметра принимает значение полного пути к запускаемому скрипту.
function OnInit(script_path)
    if is_connected then
        local msg = {}
        msg.cmd = "OnInit"
        msg.t = timemsec()
        msg.data = script_path
        sendCallback(msg)
    end
    log("Hello, QuikSharp! Running inside Quik from the path: "..getScriptPath(), 1)
end

--- Функция вызывается терминалом QUIK при получении сделки.
function OnOrder(order)
    -- TODO: почему не проверяем is_connected?
    local msg = {}
    msg.t = timemsec()
    msg.id = nil -- значение в order.trans_id
    msg.data = order
    msg.cmd = "OnOrder"
    sendCallback(msg)
end

--- Функция вызывается терминалом QUIK при получении изменения стакана котировок.
function OnQuote(class_code, sec_code)
    -- TODO: почему не проверяем is_connected?
    if true then -- is_connected
        local msg = {}
        msg.cmd = "OnQuote"
        msg.t = timemsec()
        local server_time = getInfoParam("SERVERTIME")
        local status, ql2 = pcall(getQuoteLevel2, class_code, sec_code)
        if status then
            msg.data = ql2
            msg.data.class_code = class_code
            msg.data.sec_code = sec_code
            msg.data.server_time = server_time
            sendCallback(msg)
        else
            OnError(ql2)
        end
    end
end

--- Функция вызывается терминалом QUIK при остановке скрипта из диалога управления и при закрытии терминала QUIK.
function OnStop(s)
    is_started = false

    if is_connected then
        local msg = {}
        msg.cmd = "OnStop"
        msg.t = timemsec()
        msg.data = s
        sendCallback(msg)
    end
    log("Bye, QuikSharp!")
    CleanUp()
    --	send disconnect
    return 1000
end

--- Функция вызывается терминалом QUIK при получении сделки.
function OnTrade(trade)
    -- TODO: почему не проверяем is_connected?
    local msg = {}
    msg.t = timemsec()
    msg.id = nil -- значение в OnTrade.trans_id
    msg.data = trade
    msg.cmd = "OnTrade"
    sendCallback(msg)
end

--- Функция вызывается терминалом QUIK при получении ответа на транзакцию пользователя.
function OnTransReply(trans_reply)
    -- TODO: почему не проверяем is_connected?
    local msg = {}
    msg.t = timemsec()
    msg.id = nil -- значение в trans_reply.trans_id
    msg.data = trans_reply
    msg.cmd = "OnTransReply"
    sendCallback(msg)
end

--- Функция вызывается терминалом QUIK при получении новой стоп-заявки или при изменении параметров существующей стоп-заявки..
function OnStopOrder(stop_order)
    -- TODO: почему не проверяем is_connected?
	local msg = {}
    msg.t = timemsec()
    msg.data = stop_order
    msg.cmd = "OnStopOrder"
    sendCallback(msg)
end

return qscallbacks
